using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.PitchGroup;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.PitchGroup;

namespace DMSPortal.BackendServer.Services;

public class PitchGroupsService : IPitchGroupsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PitchGroupsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<List<PitchGroupDto>> GetPitchGroupsAsync()
    {
        var pitchGroups = _unitOfWork.PitchGroups.FindAll();
        
        return _mapper.Map<List<PitchGroupDto>>(pitchGroups);
    }

    public async Task<bool> CreatePitchGroupAsync(CreatePitchGroupRequest request)
    {
        var isPitchGroupExisted =
            await _unitOfWork.PitchGroups
                .ExistAsync(x => 
                    x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
        if (isPitchGroupExisted)
            throw new BadRequestException($"PitchGroup with name {request.Name} existed");

        var pitchGroup = _mapper.Map<PitchGroup>(request);
        await _unitOfWork.PitchGroups.CreateAsync(pitchGroup);
        
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdatePitchGroupAsync(string pitchGroupId, UpdatePitchGroupRequest request)
    {
        await Task.WhenAll(new[]
        {
            new Task(async () =>
            {
                var isPitchGroupExisted =
                    await _unitOfWork.PitchGroups
                        .ExistAsync(x => x.Id.Equals(pitchGroupId));
                if (!isPitchGroupExisted)
                    throw new NotFoundException("PitchGroup does not exist");
            }),
            new Task(async () =>
            {
                var isPitchGroupExisted =
                    await _unitOfWork.PitchGroups
                        .ExistAsync(x =>
                            x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
                if (isPitchGroupExisted)
                    throw new BadRequestException($"PitchGroup with name {request.Name} existed");
            })
        });

        var pitchGroup = _mapper.Map<PitchGroup>(request);
        await _unitOfWork.PitchGroups.UpdateAsync(pitchGroup);
        
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeletePitchGroupAsync(string pitchGroupId)
    {
        var pitchGroup = await _unitOfWork.PitchGroups.GetByIdAsync(pitchGroupId);
        if (pitchGroup == null)
            throw new NotFoundException("PitchGroup does not exist");

        var isExistedBranches = await _unitOfWork.Branches
            .ExistAsync(x => x.PitchGroupId.Equals(pitchGroupId));

        if (isExistedBranches)
            throw new BadRequestException("Existing branches belong to PitchGroup");

        await _unitOfWork.PitchGroups.DeleteAsync(pitchGroup);

        return true;
    }
}