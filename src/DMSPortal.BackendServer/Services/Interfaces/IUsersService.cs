using DMSPortal.BackendServer.Models;
using DMSPortal.Models.DTOs.User;
using DMSPortal.Models.Requests.User;

namespace DMSPortal.BackendServer.Services.Interfaces;

public interface IUsersService
{
    Task<Pagination<UserDto>> GetUsersAsync(PaginationFilter filter);
    
    Task<UserDto> GetUserByIdAsync(string userId);
    
    Task<UserDto> CreateUserAsync(CreateUserRequest request);
    
    Task<bool> UpdateUserAsync(string userId, UpdateUserRequest request);
    
    Task<bool> DeleteUserAsync(string userId);
}