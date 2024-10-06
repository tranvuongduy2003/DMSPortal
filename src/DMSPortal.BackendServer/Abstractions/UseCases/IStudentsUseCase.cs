using DMSPortal.Models.Common;
using DMSPortal.Models.DTOs.Student;
using DMSPortal.Models.Requests.Student;

namespace DMSPortal.BackendServer.Abstractions.UseCases;

public interface IStudentsUseCase
{
    Task<Pagination<StudentDto>> GetStudentsAsync(PaginationFilter filter);
    
    Task<Pagination<StudentDto>> GetStudentsByClassIdAsync(string classId, PaginationFilter filter);
    
    Task<StudentDto> GetStudentByIdAsync(string studentId);
    
    Task<StudentDto> CreateStudentAsync(CreateStudentRequest request);
    
    Task<bool> UpdateStudentAsync(string studentId, UpdateStudentRequest request);
    
    Task<bool> DeleteStudentAsync(string studentId);
}