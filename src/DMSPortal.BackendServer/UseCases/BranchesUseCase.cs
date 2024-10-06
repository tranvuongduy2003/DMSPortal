using AutoMapper;
using DMSPortal.BackendServer.Abstractions.UnitOfWork;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Helpers;
using DMSPortal.Models.Common;
using DMSPortal.Models.DTOs.Branch;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.Branch;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.UseCases;

public class BranchesUseCase : IBranchesUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public BranchesUseCase(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Pagination<BranchDto>> GetBranchesAsync(PaginationFilter filter)
    {
        var branches = await _unitOfWork.Branches
            .FindAll()
            .ToListAsync();

        var pagination = PaginationHelper<Branch>.Paginate(filter, branches);

        return new Pagination<BranchDto>
        {
            Items = _mapper.Map<List<BranchDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<Pagination<BranchDto>> GetBranchesByPitchGroupIdAsync(string pitchGroupId, PaginationFilter filter)
    {
        var isPitchGroupExisted = await _unitOfWork.PitchGroups
            .ExistAsync(x => x.Id.Equals(pitchGroupId));
        if (!isPitchGroupExisted)
            throw new NotFoundException("Cụm sân không tồn tại");

        var branches = await _unitOfWork.Branches
            .FindByCondition(x => x.PitchGroupId.Equals(pitchGroupId))
            .ToListAsync();
        
        var pagination = PaginationHelper<Branch>.Paginate(filter, branches);

        return new Pagination<BranchDto>
        {
            Items = _mapper.Map<List<BranchDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<BranchDto> GetBranchByIdAsync(string branchId)
    {
        var branch = await _unitOfWork.Branches
            .FindByCondition(x => x.Id.Equals(branchId))
            .Include(x => x.Manager)
            .Include(x => x.PitchGroup)
            .FirstOrDefaultAsync();
        
        if (branch == null)
            throw new NotFoundException("Chi nhánh không tồn tại");
        
        return _mapper.Map<BranchDto>(branch);
    }

    public async Task<BranchDto> CreateBranchAsync(CreateBranchRequest request)
    {
        var isBranchExisted =
            await _unitOfWork.Branches
                .ExistAsync(x =>
                    x.Name.Equals(request.Name));
        if (isBranchExisted)
            throw new BadRequestException($"Chi nhánh {request.Name} đã tồn tại");

        var pitchGroup = await _unitOfWork.PitchGroups.GetByIdAsync(request.PitchGroupId);
        if (pitchGroup == null)
            throw new NotFoundException($"Cụm sân không tồn tại");

        var manager = await _userManager.FindByIdAsync(request.ManagerId);
        if (manager == null)
            throw new NotFoundException($"Quản lý không tồn tại");

        var branch = _mapper.Map<Branch>(request);
        await _unitOfWork.Branches.CreateAsync(branch);

        pitchGroup.NumberOfBranches++;
        await _unitOfWork.PitchGroups.UpdateAsync(pitchGroup);

        manager.NumberOfBranches++;
        await _userManager.UpdateAsync(manager);

        await _unitOfWork.CommitAsync();

        return  _mapper.Map<BranchDto>(branch);
    }

    public async Task<bool> UpdateBranchAsync(string branchId, UpdateBranchRequest request)
    {
        var branch = await _unitOfWork.Branches.GetByIdAsync(branchId);
        if (branch == null)
            throw new NotFoundException($"Chi nhánh không tồn tại");
        
        // var isBranchExistedByName =
        //     await _unitOfWork.Branches
        //         .ExistAsync(x => x.Name.Equals(request.Name));
        // if (isBranchExistedByName)
        //     throw new BadRequestException($"Chi nhánh {request.Name} đã tồn tại");
        
        var manager = await _userManager.FindByIdAsync(request.ManagerId);
        if (manager == null)
            throw new NotFoundException($"Cụm sân không tồn tại");

        branch.Name = request.Name;
        branch.Address = request.Address;
        branch.Status = request.Status;
        branch.ManagerId = request.ManagerId;
        await _unitOfWork.Branches.UpdateAsync(branch);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeleteBranchAsync(string branchId)
    {
        var branch = await _unitOfWork.Branches.GetByIdAsync(branchId);
        if (branch == null)
            throw new NotFoundException($"Chi nhánh không tồn tại");

        var isExistedPitches = await _unitOfWork.Pitches
            .ExistAsync(x => x.BranchId.Equals(branchId));
        if (isExistedPitches)
            throw new BadRequestException("Chi nhánh không trống, không thể xóa Chi nhánh");

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