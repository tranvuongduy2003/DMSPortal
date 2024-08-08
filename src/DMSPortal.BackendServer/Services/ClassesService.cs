using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Class;
using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.Class;

namespace DMSPortal.BackendServer.Services;

public class ClassesService : IClassesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ClassesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<List<ClassDto>> GetClassesAsync()
    {
        var classes = _unitOfWork.Classes.FindAll();

        return _mapper.Map<List<ClassDto>>(classes);
    }

    public async Task<List<ClassDto>> GetClassesByPitchIdAsync(string pitchId)
    {
        var isPitchExisted = await _unitOfWork.Pitches
            .ExistAsync(x => x.Id.Equals(pitchId));
        if (!isPitchExisted)
            throw new NotFoundException("Pitch does not exist");

        var classes = _unitOfWork.Classes.FindByCondition(
            x => x.PitchId.Equals(pitchId));

        return _mapper.Map<List<ClassDto>>(classes);
    }

    public async Task<bool> CreateClassAsync(CreateClassRequest request)
    {
        var isClassExisted =
            await _unitOfWork.Classes
                .ExistAsync(x =>
                    x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
        if (isClassExisted)
            throw new BadRequestException($"Class with name {request.Name} existed");

        var pitch = await _unitOfWork.Pitches.GetByIdAsync(request.PitchId);
        if (pitch == null)
            throw new NotFoundException($"Pitch with id {request.PitchId} does not exist");

        var classData = _mapper.Map<Class>(request);
        await _unitOfWork.Classes.CreateAsync(classData);

        pitch.NumberOfClasses++;
        await _unitOfWork.Pitches.UpdateAsync(pitch);

        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateClassAsync(string classId, UpdateClassRequest request)
    {
        await Task.WhenAll(new[]
        {
            new Task(async () =>
            {
                var isClassExisted =
                    await _unitOfWork.Classes
                        .ExistAsync(x => x.Id.Equals(classId));
                if (!isClassExisted)
                    throw new NotFoundException($"Class with id {classId} does not exist");
            }),
            new Task(async () =>
            {
                var isClassExisted =
                    await _unitOfWork.Classes
                        .ExistAsync(x =>
                            x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
                if (isClassExisted)
                    throw new BadRequestException($"Class with name {request.Name} existed");
            }),
        });

        var classData = _mapper.Map<Class>(request);
        await _unitOfWork.Classes.UpdateAsync(classData);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteClassAsync(string classId)
    {
        var classData = await _unitOfWork.Classes.GetByIdAsync(classId);
        if (classData == null)
            throw new NotFoundException($"Class with id {classId} does not exist");

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