using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Services.Interfaces;
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

    public async Task<List<StudentDto>> GetStudentsAsync()
    {
        var students = _unitOfWork.Students.FindAll();

        return _mapper.Map<List<StudentDto>>(students);
    }

    public async Task<List<StudentDto>> GetStudentsByClassIdAsync(string classId)
    {
        var isClassExisted = await _unitOfWork.Classes
            .ExistAsync(x => x.Id.Equals(classId));
        if (!isClassExisted)
            throw new NotFoundException("Class does not exist");

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

        return _mapper.Map<List<StudentDto>>(studentsInClass);
    }

    public async Task<StudentDto> GetStudentByIdAsync(string studentId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId);
        if (student == null)
            throw new NotFoundException("Student does not exist");

        var notes = await _unitOfWork.Notes
            .FindByCondition(x => x.StudentId.Equals(studentId))
            .ToListAsync();

        student.Notes = notes;
        return _mapper.Map<StudentDto>(student);
    }

    public async Task<bool> CreateStudentAsync(CreateStudentRequest request)
    {
        var student = _mapper.Map<Student>(request);
        await _unitOfWork.Students.CreateAsync(student);

        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateStudentAsync(string studentId, UpdateStudentRequest request)
    {
        var isStudentExisted =
            await _unitOfWork.Students
                .ExistAsync(x => x.Id.Equals(studentId));
        if (!isStudentExisted)
            throw new NotFoundException($"Student with id {studentId} does not exist");

        var student = _mapper.Map<Student>(request);
        await _unitOfWork.Students.UpdateAsync(student);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteStudentAsync(string studentId)
    {
        var student = await _unitOfWork.Students.GetByIdAsync(studentId);
        if (student == null)
            throw new NotFoundException($"Student with id {studentId} does not exist");

        await _unitOfWork.Students.DeleteAsync(student);

        await Task.WhenAll(new[]
        {
            new Task(async () =>
            {
                var notes = _unitOfWork.Notes
                    .FindByCondition(x =>
                        x.StudentId.Equals(student.Id));
                await _unitOfWork.Notes.DeleteListAsync(notes);
            }),
            new Task(async () =>
            {
                var attendances = _unitOfWork.Attendances
                    .FindByCondition(x =>
                        x.StudentId.Equals(student.Id));
                await _unitOfWork.Attendances.DeleteListAsync(attendances);
            }),
            new Task(async () =>
            {
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

                await Task.WhenAll(studentClasses.Select(studentClass => new Task(async () =>
                {
                    studentClass.NumberOfStudents--;
                    await _unitOfWork.Classes.UpdateAsync(studentClass);
                })));

                await _unitOfWork.StudentInClasses.DeleteListAsync(studentInClasses);
            }),
        });

        await _unitOfWork.CommitAsync();

        return true;
    }
}