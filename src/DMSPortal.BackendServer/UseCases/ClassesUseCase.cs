using AutoMapper;
using DMSPortal.BackendServer.Abstractions.UnitOfWork;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Helpers;
using DMSPortal.Models.Common;
using DMSPortal.Models.DTOs.Class;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.Class;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.UseCases;

public class ClassesUseCase : IClassesUseCase
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClassesUseCase(UserManager<User> userManager, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<ClassDto>> GetClassesAsync(PaginationFilter filter)
    {
        var classes = await _unitOfWork.Classes
            .FindAll()
            .Include(x => x.Pitch)
            .Include(x => x.Teacher)
            .ToListAsync();

        var pagination = PaginationHelper<Class>.Paginate(filter, classes);

        return new Pagination<ClassDto>
        {
            Items = _mapper.Map<List<ClassDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<Pagination<ClassDto>> GetClassesByPitchIdAsync(string pitchId, PaginationFilter filter)
    {
        var isPitchExisted = await _unitOfWork.Pitches
            .ExistAsync(x => x.Id.Equals(pitchId));
        if (!isPitchExisted)
            throw new NotFoundException("Sân không tồn tại");

        var classes = await _unitOfWork.Classes
            .FindByCondition(x => x.PitchId.Equals(pitchId))
            .Include(x => x.Pitch)
            .Include(x => x.Teacher)
            .ToListAsync();

        var pagination = PaginationHelper<Class>.Paginate(filter, classes);

        return new Pagination<ClassDto>
        {
            Items = _mapper.Map<List<ClassDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<ClassDto> GetClassByIdAsync(string classId)
    {
        var classEntity = await _unitOfWork.Classes
            .FindByCondition(x => x.Id.Equals(classId))
            .Include(x => x.Pitch)
            .Include(x => x.Teacher)
            .FirstOrDefaultAsync();

        if (classEntity == null)
            throw new NotFoundException("Lớp không tồn tại");

        return _mapper.Map<ClassDto>(classEntity);
    }

    public async Task<ClassDto> CreateClassAsync(CreateClassRequest request)
    {
        var isClassExisted =
            await _unitOfWork.Classes
                .ExistAsync(x => x.Name.Equals(request.Name));
        if (isClassExisted)
            throw new BadRequestException($"Lớp {request.Name} đã tồn tại");

        var pitch = await _unitOfWork.Pitches.GetByIdAsync(request.PitchId);
        if (pitch == null)
            throw new NotFoundException($"Sân không tồn tại");

        var teacher = await _userManager.FindByIdAsync(request.TeacherId);
        if (teacher == null)
            throw new NotFoundException($"Giáo viên không tồn tại");

        var classData = _mapper.Map<Class>(request);
        await _unitOfWork.Classes.CreateAsync(classData);

        pitch.NumberOfClasses++;
        await _unitOfWork.Pitches.UpdateAsync(pitch);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<ClassDto>(classData);
    }

    public async Task<bool> UpdateClassAsync(string classId, UpdateClassRequest request)
    {
        var classData = await _unitOfWork.Classes.GetByIdAsync(classId);
        if (classData == null)
            throw new NotFoundException($"Lớp không tồn tại");

        var isClassExisted =
            await _unitOfWork.Classes
                .ExistAsync(x => x.Name.Equals(request.Name));
        if (isClassExisted)
            throw new BadRequestException($"Lớp {request.Name} đã tổn tại");

        var teacher = await _userManager.FindByIdAsync(request.TeacherId);
        if (teacher == null)
            throw new NotFoundException($"Giáo viên không tồn tại");

        classData.Name = request.Name;
        classData.Status = request.Status;
        classData.TeacherId = request.TeacherId;
        await _unitOfWork.Classes.UpdateAsync(classData);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteClassAsync(string classId)
    {
        var classData = await _unitOfWork.Classes.GetByIdAsync(classId);
        if (classData == null)
            throw new NotFoundException($"Lớp không tồn tại");

        await _unitOfWork.Classes.DeleteAsync(classData);

        await Task.WhenAll(new[]
        {
            new Task(async () =>
            {
                var studentInClasses = _unitOfWork.StudentInClasses
                    .FindByCondition(x =>
                        x.ClassId.Equals(classData.Id));
                await _unitOfWork.StudentInClasses.DeleteListAsync(studentInClasses);
            }),
            new Task(async () =>
            {
                var attendances = _unitOfWork.Attendances
                    .FindByCondition(x =>
                        x.ClassId.Equals(classData.Id));
                await _unitOfWork.Attendances.DeleteListAsync(attendances);
            }),
            new Task(async () =>
            {
                var classInShifts = _unitOfWork.ClassInShifts
                    .FindByCondition(x =>
                        x.ClassId.Equals(classData.Id));
                await _unitOfWork.ClassInShifts.DeleteListAsync(classInShifts);
            }),
            new Task(async () =>
            {
                var pitch = await _unitOfWork.Pitches.GetByIdAsync(classData.PitchId);
                if (pitch != null)
                {
                    pitch.NumberOfClasses--;
                    await _unitOfWork.Pitches.UpdateAsync(pitch);
                }
            })
        });

        await _unitOfWork.CommitAsync();

        return true;
    }
}