using AutoMapper;
using DMSPortal.BackendServer.Abstractions.UnitOfWork;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.Models.DTOs.Command;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.UseCases;

public class CommandsUseCase : ICommandsUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CommandsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
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