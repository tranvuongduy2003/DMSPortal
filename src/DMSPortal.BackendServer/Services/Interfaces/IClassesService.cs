using DMSPortal.BackendServer.Models;
using DMSPortal.Models.DTOs.Class;
using DMSPortal.Models.Requests.Class;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IClassesService
{
    Task<Pagination<ClassDto>> GetClassesAsync(PaginationFilter filter);
    
    Task<Pagination<ClassDto>> GetClassesByPitchIdAsync(string pitchId, PaginationFilter filter);
    
    Task<ClassDto> GetClassByIdAsync(string classId);
    
    Task<ClassDto> CreateClassAsync(CreateClassRequest request);
    
    Task<bool> UpdateClassAsync(string classId, UpdateClassRequest request);
    
    Task<bool> DeleteClassAsync(string classId);
}