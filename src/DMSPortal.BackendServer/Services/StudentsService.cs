using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Helpers;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Models;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Note;
using DMSPortal.Models.DTOs.Student;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.Student;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.Services;

public class StudentsService : IStudentsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<StudentDto>> GetStudentsAsync(PaginationFilter filter)
    {
        var students = await _unitOfWork.Students
            .FindAll()
            .ToListAsync();

        var pagination = PaginationHelper<Student>.Paginate(filter, students);

        return new Pagination<StudentDto>
        {
            Items = _mapper.Map<List<StudentDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<Pagination<StudentDto>> GetStudentsByClassIdAsync(string classId, PaginationFilter filter)
    {
        var isClassExisted = await _unitOfWork.Classes
            .ExistAsync(x => x.Id.Equals(classId));
        if (!isClassExisted)
            throw new NotFoundException("Lớp không tồn tại");

        var studentInClasses = await _unitOfWork.StudentInClasses
            .FindAll()
            .ToListAsync();

        var students = await _unitOfWork.Students
            .FindAll()
            .ToListAsync();

        var studentsInClass = (
                from _student in students
                join _studentInClass in studentInClasses on _student.Id equals _studentInClass.StudentId
                where _studentInClass.ClassId.Equals(classId)
                select _student)
            .ToList();

        var pagination = PaginationHelper<Student>.Paginate(filter, studentsInClass);

        return new Pagination<StudentDto>
        {
            Items = _mapper.Map<List<StudentDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<StudentDto> GetStudentByIdAsync(string studentId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId);
        if (student == null)
            throw new NotFoundException("Học viên không tồn tại");

        var notes = await _unitOfWork.Notes
            .FindByCondition(x => x.StudentId.Equals(studentId))
            .ToListAsync();

        student.Notes = notes;
        return _mapper.Map<StudentDto>(student);
    }

    public async Task<StudentDto> CreateStudentAsync(CreateStudentRequest request)
    {
        var student = _mapper.Map<Student>(request);
        await _unitOfWork.Students.CreateAsync(student);
        var studentDto = _mapper.Map<StudentDto>(student);

        if (request.Note != null)
        {
            var note = _mapper.Map<Note>(request.Note);
            note.StudentId = student.Id;
            await _unitOfWork.Notes.CreateAsync(note);
            studentDto.Notes = new List<NoteDto>()
            {
                _mapper.Map<NoteDto>(note)
            };
        }

        await _unitOfWork.CommitAsync();

        return studentDto;
    }

    public async Task<bool> UpdateStudentAsync(string studentId, UpdateStudentRequest request)
    {
        var isStudentExisted =
            await _unitOfWork.Students
                .ExistAsync(x => x.Id.Equals(studentId));
        if (!isStudentExisted)
            throw new NotFoundException($"Học viên không tồn tại");

        var student = _mapper.Map<Student>(request);
        await _unitOfWork.Students.UpdateAsync(student);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteStudentAsync(string studentId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId);
        if (student == null)
            throw new NotFoundException($"Học viên không tồn tại");

        await _unitOfWork.Students.DeleteAsync(student);
        
        var notes = _unitOfWork.Notes
            .FindByCondition(x =>
                x.StudentId.Equals(student.Id));
        await _unitOfWork.Notes.DeleteListAsync(notes);
        
        var attendances = _unitOfWork.Attendances
            .FindByCondition(x =>
                x.StudentId.Equals(student.Id));
        await _unitOfWork.Attendances.DeleteListAsync(attendances);

        var studentInClasses = await _unitOfWork.StudentInClasses
            .FindByCondition(x =>
                x.StudentId.Equals(student.Id))
            .ToListAsync();
        var classes = await _unitOfWork.Classes
            .FindAll()
            .ToListAsync();
        var studentClasses = classes
            .Join(
                studentInClasses,
                _class => _class.Id,
                _studentInClass => _studentInClass.ClassId,
                (_class, _) => _class);

        foreach (var studentClass in studentClasses)
        {
            studentClass.NumberOfStudents--;
            await _unitOfWork.Classes.UpdateAsync(studentClass);
        }


        await _unitOfWork.StudentInClasses.DeleteListAsync(studentInClasses);

        await _unitOfWork.CommitAsync();

        return true;
    }
}