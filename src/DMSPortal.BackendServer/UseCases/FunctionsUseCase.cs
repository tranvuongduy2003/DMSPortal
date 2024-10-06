using AutoMapper;
using DMSPortal.BackendServer.Abstractions.UnitOfWork;
using DMSPortal.BackendServer.Abstractions.UseCases;
using DMSPortal.Models.DTOs.Function;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.UseCases;

public class FunctionsUseCase : IFunctionsUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FunctionsUseCase(IUnitOfWork unitOfWork, IMapper mapper)
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