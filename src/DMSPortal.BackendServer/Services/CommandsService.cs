using AutoMapper;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs;
using DMSPortal.Models.DTOs.Command;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.Services;

public class CommandsService : ICommandsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommandsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CommandDto>> GetAllCommandsAsync()
    {
        try
        {
            var commands = await  _unitOfWork.Commands.FindAll().ToListAsync();
            return _mapper.Map<List<CommandDto>>(commands);
        }
        catch (Exception)
        {
            throw;
        }
    }
}