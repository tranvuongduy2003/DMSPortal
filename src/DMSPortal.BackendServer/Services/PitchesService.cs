using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.Pitch;

namespace DMSPortal.BackendServer.Services;

public class PitchesService : IPitchesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PitchesService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<PitchDto>> GetPitchesAsync()
    {
        var pitches = _unitOfWork.Pitches.FindAll();

        return _mapper.Map<List<PitchDto>>(pitches);
    }

    public async Task<List<PitchDto>> GetPitchesByBranchIdAsync(string branchId)
    {
        var isBranchExisted = await _unitOfWork.Branches
            .ExistAsync(x => x.Id.Equals(branchId));
        if (!isBranchExisted)
            throw new NotFoundException("Branch does not exist");

        var pitches = _unitOfWork.Pitches.FindByCondition(
            x => x.BranchId.Equals(branchId));

        return _mapper.Map<List<PitchDto>>(pitches);
    }

    public async Task<bool> CreatePitchAsync(CreatePitchRequest request)
    {
        var isPitchExisted =
            await _unitOfWork.Pitches
                .ExistAsync(x =>
                    x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
        if (isPitchExisted)
            throw new BadRequestException($"Pitch with name {request.Name} existed");

        var branch = await _unitOfWork.Branches.GetByIdAsync(request.BranchId);
        if (branch == null)
            throw new NotFoundException($"Branch with id {request.BranchId} does not exist");

        var pitch = _mapper.Map<Pitch>(request);
        await _unitOfWork.Pitches.CreateAsync(pitch);

        branch.NumberOfPitches++;
        await _unitOfWork.Branches.UpdateAsync(branch);

        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdatePitchAsync(string pitchId, UpdatePitchRequest request)
    {
        await Task.WhenAll(new[]
        {
            new Task(async () =>
            {
                var isPitchExisted =
                    await _unitOfWork.Pitches
                        .ExistAsync(x => x.Id.Equals(pitchId));
                if (!isPitchExisted)
                    throw new NotFoundException($"Pitch with id {pitchId} does not exist");
            }),
            new Task(async () =>
            {
                var isPitchExisted =
                    await _unitOfWork.Pitches
                        .ExistAsync(x =>
                            x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
                if (isPitchExisted)
                    throw new BadRequestException($"Pitch with name {request.Name} existed");
            }),
        });

        var pitch = _mapper.Map<Pitch>(request);
        await _unitOfWork.Pitches.UpdateAsync(pitch);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeletePitchAsync(string pitchId)
    {
        var pitch = await _unitOfWork.Pitches.GetByIdAsync(pitchId);
        if (pitch == null)
            throw new NotFoundException($"Pitch with id {pitchId} does not exist");

        var isExistedClasses = await _unitOfWork.Classes
            .ExistAsync(x => x.PitchId.Equals(pitchId));
        if (isExistedClasses)
            throw new BadRequestException("Existing classes belong to Pitch");

        var branch = await _unitOfWork.Branches.GetByIdAsync(pitch.BranchId);

        await _unitOfWork.Pitches.DeleteAsync(pitch);

        if (branch != null)
        {
            branch.NumberOfPitches--;
            await _unitOfWork.Branches.UpdateAsync(branch);
        }

        await _unitOfWork.CommitAsync();

        return true;
    }
}