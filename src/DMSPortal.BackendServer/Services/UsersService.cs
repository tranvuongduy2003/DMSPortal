using AutoMapper;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Helpers;
using DMSPortal.BackendServer.Models;
using DMSPortal.BackendServer.Services.Interfaces;
using DMSPortal.Models.DTOs.User;
using DMSPortal.Models.Exceptions;
using DMSPortal.Models.Requests.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.Services;

public class UsersService : IUsersService
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public UsersService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Pagination<UserDto>> GetUsersAsync(PaginationFilter filter)
    {
        var users = await _userManager.Users
            .AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .ToListAsync();

        var pagination = PaginationHelper<User>.Paginate(filter, users);

        return new Pagination<UserDto>
        {
            Items = _mapper.Map<List<UserDto>>(pagination.Items),
            Metadata = pagination.Metadata
        };
    }

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null || user.DeletedAt != null)
            throw new NotFoundException("Người dùng không tồn tại");

        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
    {
        var isUserExisted =
            await _userManager.Users.AsNoTracking()
                .AnyAsync(x => x.DeletedAt == null && (x.UserName.Equals(request.UserName)
                               || x.Email.Equals(request.Email)
                               || x.PhoneNumber.Equals(request.PhoneNumber)));
        if (isUserExisted)
            throw new BadRequestException($"Email, số điện thoại hoặc username đã tồn tại");

        var user = _mapper.Map<User>(request);
        var result = await _userManager.CreateAsync(user);

        if (result.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, request.Roles);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = request.Roles;
            return userDto;
        }
        else
        {
            throw new BadRequestException(result);
        }
    }

    public async Task<bool> UpdateUserAsync(string userId, UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.DeletedAt != null)
            throw new NotFoundException($"Người dùng không tồn tại");
        
        user.UserName = request.UserName;
        user.Email = request.Email;
        user.PhoneNumber = request.PhoneNumber;
        user.FullName = request.FullName;
        user.Dob = request.Dob;
        user.Gender = request.Gender;
        user.Avatar = request.Avatar;
        user.Address = request.Address;

        var result = await _userManager.UpdateAsync(user);
        
        if (result.Succeeded)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, request.Roles);
            return true;
        }
        else
        {
            throw new BadRequestException(result);
        }
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.DeletedAt != null)
            throw new NotFoundException($"Người dùng không tồn tại");
        
        user.DeletedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}