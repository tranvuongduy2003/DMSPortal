using AutoMapper;
using DMSPortal.BackendServer.Infrastructure.Interfaces;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs;
using DMSPortal.Models.DTOs.Function;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.Services;

public class FunctionsService : IFunctionsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FunctionsService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<List<FunctionDto>> GetAllFunctionsAsync()
    {
        try
        {
            var functions = await  _unitOfWork.Functions.FindAll().ToListAsync();
            return _mapper.Map<List<FunctionDto>>(functions);
        }
        catch (Exception)
        {
            throw;
        }
    }
}