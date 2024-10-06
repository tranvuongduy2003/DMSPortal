using AutoMapper;
using DMSPortal.BackendServer.Abstractions.UnitOfWork;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Helpers;
using DMSPortal.Models.Common;
using DMSPortal.Models.DTOs.Pitch;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.Pitch;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.UseCases;

public class PitchesUseCase : IPitchesUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PitchesUseCase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<PitchDto>> GetPitchesAsync(PaginationFilter filter)
    {
        var pitches = await _unitOfWork.Pitches
            .FindAll()
            .ToListAsync();

        var pagination = PaginationHelper<Pitch>.Paginate(filter, pitches);

        return new Pagination<PitchDto>
        {
            Items = _mapper.Map<List<PitchDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<Pagination<PitchDto>> GetPitchesByBranchIdAsync(string branchId, PaginationFilter filter)
    {
        var isBranchExisted = await _unitOfWork.Branches
            .ExistAsync(x => x.Id.Equals(branchId));
        if (!isBranchExisted)
            throw new NotFoundException("Chi nhánh không tồn tại");

        var pitches = await _unitOfWork.Pitches
            .FindByCondition(x => x.BranchId.Equals(branchId))
            .ToListAsync();

        var pagination = PaginationHelper<Pitch>.Paginate(filter, pitches);

        return new Pagination<PitchDto>
        {
            Items = _mapper.Map<List<PitchDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<PitchDto> GetPitchByIdAsync(string pitchId)
    {
        var pitch = await _unitOfWork.Pitches
            .FindByCondition(x => x.Id.Equals(pitchId))
            .Include(x => x.Branch)
            .FirstOrDefaultAsync();

        if (pitch == null)
            throw new NotFoundException("Sân không tồn tại");

        return _mapper.Map<PitchDto>(pitch);
    }

    public async Task<PitchDto> CreatePitchAsync(CreatePitchRequest request)
    {
        var isPitchExisted =
            await _unitOfWork.Pitches
                .ExistAsync(x => x.Name.Equals(request.Name));
        if (isPitchExisted)
            throw new BadRequestException($"Sân {request.Name} đã tổn tại");

        var branch = await _unitOfWork.Branches.GetByIdAsync(request.BranchId);
        if (branch == null)
            throw new NotFoundException($"Chi nhánh không tồn tại");

        var pitch = _mapper.Map<Pitch>(request);
        await _unitOfWork.Pitches.CreateAsync(pitch);

        branch.NumberOfPitches++;
        await _unitOfWork.Branches.UpdateAsync(branch);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<PitchDto>(pitch);
    }

    public async Task<bool> UpdatePitchAsync(string pitchId, UpdatePitchRequest request)
    {
        var pitch = await _unitOfWork.Pitches.GetByIdAsync(pitchId);
        if (pitch == null)
            throw new NotFoundException($"Sân không tồn tại");

        var isPitchExistedByName =
            await _unitOfWork.Pitches
                .ExistAsync(x =>
                    x.Name.Equals(request.Name));
        if (isPitchExistedByName)
            throw new BadRequestException($"Sân {request.Name} đã tổn tại");

        pitch.Name = request.Name;
        pitch.Status = request.Status;
        await _unitOfWork.Pitches.UpdateAsync(pitch);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeletePitchAsync(string pitchId)
    {
        var pitch = await _unitOfWork.Pitches.GetByIdAsync(pitchId);
        if (pitch == null)
            throw new NotFoundException($"Sân không tồn tại");

        var isExistedClasses = await _unitOfWork.Classes
            .ExistAsync(x => x.PitchId.Equals(pitchId));
        if (isExistedClasses)
            throw new BadRequestException("Sân vẫn còn chứa Lớp, không thể xóa Sân");

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