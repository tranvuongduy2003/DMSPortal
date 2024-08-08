using DMSPortal.Models.DTOs.Student;
using DMSPortal.Models.Models;
using DMSPortal.Models.Requests.Student;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IStudentsService
{
    Task<Pagination<StudentDto>> GetStudentsAsync(PaginationFilter filter);
    
    Task<Pagination<StudentDto>> GetStudentsByClassIdAsync(string classId, PaginationFilter filter);
    
    Task<StudentDto> GetStudentByIdAsync(string studentId);
    
    Task<bool> CreateStudentAsync(CreateStudentRequest request);
    
    Task<bool> UpdateStudentAsync(string studentId, UpdateStudentRequest request);
    
    Task<bool> DeleteStudentAsync(string studentId);
}