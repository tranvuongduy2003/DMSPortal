using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Helpers;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Models;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.PitchGroup;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.PitchGroup;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<Pagination<PitchGroupDto>> GetPitchGroupsAsync(PaginationFilter filter)
    {
        var pitchGroups = await _unitOfWork.PitchGroups
            .FindAll()
            .ToListAsync();
        
        var pagination = PaginationHelper<PitchGroup>.Paginate(filter, pitchGroups);

        return new Pagination<PitchGroupDto>
        {
            Items = _mapper.Map<List<PitchGroupDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<PitchGroupDto> GetPitchGroupByIdAsync(string pitchGroupId)
    {
        var pitchGroup = await _unitOfWork.PitchGroups.GetByIdAsync(pitchGroupId);
        
        if (pitchGroup == null)
            throw new NotFoundException("Cụm sân không tồn tại");
        
        return _mapper.Map<PitchGroupDto>(pitchGroup);
    }

    public async Task<PitchGroupDto> CreatePitchGroupAsync(CreatePitchGroupRequest request)
    {
        var isPitchGroupExisted =
            await _unitOfWork.PitchGroups
                .ExistAsync(x => 
                    x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
        if (isPitchGroupExisted)
            throw new BadRequestException($"Cụm sân {request.Name} đã tổn tại");

        var pitchGroup = _mapper.Map<PitchGroup>(request);
        await _unitOfWork.PitchGroups.CreateAsync(pitchGroup);
        
        await _unitOfWork.CommitAsync();

        return _mapper.Map<PitchGroupDto>(pitchGroup);
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
                    throw new NotFoundException("Cụm sân không tồn tại");
            }),
            new Task(async () =>
            {
                var isPitchGroupExisted =
                    await _unitOfWork.PitchGroups
                        .ExistAsync(x =>
                            x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase));
                if (isPitchGroupExisted)
                    throw new BadRequestException($"Cụm sân {request.Name} đã tổn tại");
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
            throw new NotFoundException("Cụm sân không tồn tại");

        var isExistedBranches = await _unitOfWork.Branches
            .ExistAsync(x => x.PitchGroupId.Equals(pitchGroupId));

        if (isExistedBranches)
            throw new BadRequestException("Cụm sân vẫn còn chứa Sân, không thể xóa Cụm sân");

        await _unitOfWork.PitchGroups.DeleteAsync(pitchGroup);

        return true;
    }
}