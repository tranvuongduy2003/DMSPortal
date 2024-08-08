using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Models;
using DMSPortal.Models.Requests.Branch;
using Microsoft.AspNetCore.Identity;

namespace DMSPortal.BackendServer.Services;

public class BranchesService : IBranchesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public BranchesService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Pagination<BranchDto>> GetBranchesAsync(PaginationFilter filter)
    {
        var branches = _unitOfWork.Branches.FindAll();

        return _mapper.Map<Pagination<BranchDto>>(branches);
    }

    public async Task<Pagination<BranchDto>> GetBranchesByPitchGroupIdAsync(string pitchGroupId, PaginationFilter filter)
    {
        var isPitchGroupExisted = await _unitOfWork.PitchGroups
            .ExistAsync(x => x.Id.Equals(pitchGroupId));
        if (!isPitchGroupExisted)
            throw new NotFoundException("PitchGroup does not exist");

        var branches = _unitOfWork.Branches.FindByCondition(
            x => x.PitchGroupId.Equals(pitchGroupId));

        return _mapper.Map<List<BranchDto>>(branches);
    }

    public async Task<bool> CreateBranchAsync(CreateBranchRequest request)
    {
        var isBranchExisted =
            await _unitOfWork.Branches
                .ExistAsync(x =>
                    x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
        if (isBranchExisted)
            throw new BadRequestException($"Branch with name {request.Name} existed");

        var pitchGroup = await _unitOfWork.PitchGroups.GetByIdAsync(request.PitchGroupId);
        if (pitchGroup == null)
            throw new NotFoundException($"PitchGroup with id {request.PitchGroupId} does not exist");

        var manager = await _userManager.FindByIdAsync(request.ManagerId);
        if (manager == null)
            throw new NotFoundException($"Manager with id {request.ManagerId} does not exist");

        var branch = _mapper.Map<Branch>(request);
        await _unitOfWork.Branches.CreateAsync(branch);

        pitchGroup.NumberOfBranches++;
        await _unitOfWork.PitchGroups.UpdateAsync(pitchGroup);

        manager.NumberOfBranches++;
        await _userManager.UpdateAsync(manager);

        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> UpdateBranchAsync(string branchId, UpdateBranchRequest request)
    {
        await Task.WhenAll(new[]
        {
            new Task(async () =>
            {
                var isBranchExisted =
                    await _unitOfWork.Branches
                        .ExistAsync(x => x.Id.Equals(branchId));
                if (!isBranchExisted)
                    throw new NotFoundException($"Branch with id {branchId} does not exist");
            }),
            new Task(async () =>
            {
                var isBranchExisted =
                    await _unitOfWork.Branches
                        .ExistAsync(x =>
                            x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
                if (isBranchExisted)
                    throw new BadRequestException($"Branch with name {request.Name} existed");
            }),
            new Task(async () =>
            {
                var manager = await _userManager.FindByIdAsync(request.ManagerId);
                if (manager == null)
                    throw new NotFoundException($"Manager with id {request.ManagerId} does not exist");
            })
        });

        var branch = _mapper.Map<Branch>(request);
        await _unitOfWork.Branches.UpdateAsync(branch);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteBranchAsync(string branchId)
    {
        var branch = await _unitOfWork.Branches.GetByIdAsync(branchId);
        if (branch == null)
            throw new NotFoundException($"Branch with id {branchId} does not exist");

        var isExistedPitches = await _unitOfWork.Pitches
            .ExistAsync(x => x.BranchId.Equals(branchId));
        if (isExistedPitches)
            throw new BadRequestException("Existing pitches belong to Branch");

        var pitchGroup = await _unitOfWork.PitchGroups.GetByIdAsync(branch.PitchGroupId);

        var manager = await _userManager.FindByIdAsync(branch.ManagerId);

        await _unitOfWork.Branches.DeleteAsync(branch);

        if (pitchGroup != null)
        {
            pitchGroup.NumberOfBranches--;
            await _unitOfWork.PitchGroups.UpdateAsync(pitchGroup);
        }

        if (manager != null)
        {
            manager.NumberOfBranches--;
            await _userManager.UpdateAsync(manager);
        }

        await _unitOfWork.CommitAsync();

        return true;
    }
}