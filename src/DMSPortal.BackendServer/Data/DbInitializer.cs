using Bogus;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;
using Function = DMSPortal.BackendServer.Data.Entities.Function;

namespace DMSPortal.BackendServer.Data;

public class DbInitializer
{
    private const int MAX_USERS_QUANTITY = 10;

    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public DbInitializer(ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task Seed()
    {
        SeedRoles().Wait();
        SeedUsers().Wait();
        SeedFunctions().Wait();
        SeedCommands().Wait();
        SeedPermission().Wait();
    }

    private async Task SeedRoles()
    {
        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = EUserRole.ADMIN.GetDisplayName(),
                ConcurrencyStamp = "1",
                NormalizedName = EUserRole.ADMIN.GetDisplayName().Normalize()
            });
            await _roleManager.CreateAsync(new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = EUserRole.CUSTOMER.GetDisplayName(),
                ConcurrencyStamp = "2",
                NormalizedName = EUserRole.CUSTOMER.GetDisplayName().Normalize()
            });
            await _roleManager.CreateAsync(new Role()
            {
                Id = Guid.NewGuid().ToString(),
                Name = EUserRole.SHIPPER.GetDisplayName(),
                ConcurrencyStamp = "3",
                NormalizedName = EUserRole.SHIPPER.GetDisplayName().Normalize()
            });
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedUsers()
    {
        if (!_userManager.Users.Any())
        {
            User admin = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com",
                UserName = "admin",
                NormalizedUserName = "admin",
                LockoutEnabled = false,
                PhoneNumber = "0829440357",
                FullName = "Admin",
                Dob = new Faker().Person.DateOfBirth,
                Gender = EGender.MALE,
                Bio = new Faker().Lorem.ToString(),
                Status = EUserStatus.ACTIVE,
            };
            var result = await _userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync("admin@gmail.com");
                await _userManager.AddToRoleAsync(user, EUserRole.ADMIN.GetDisplayName());
            }

            var userFaker = new Faker<User>()
                .RuleFor(u => u.Id, _ => Guid.NewGuid().ToString())
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.NormalizedEmail, f => f.Person.Email)
                .RuleFor(u => u.UserName, f => f.Person.UserName)
                .RuleFor(u => u.NormalizedUserName, f => f.Person.UserName)
                .RuleFor(u => u.LockoutEnabled, f => false)
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("###-###-####"))
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Dob, f => f.Person.DateOfBirth)
                .RuleFor(u => u.Gender, f => f.PickRandom<EGender>())
                .RuleFor(u => u.Bio, f => f.Lorem.ToString())
                .RuleFor(u => u.Status, _ => EUserStatus.ACTIVE);

            for (int userIndex = 0; userIndex < MAX_USERS_QUANTITY; userIndex++)
            {
                var customer = userFaker.Generate();
                var customerResult = await _userManager.CreateAsync(customer, "Customer@123");
                if (customerResult.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(customer.Email);
                    await _userManager.AddToRolesAsync(user, new List<string> { EUserRole.CUSTOMER.GetDisplayName() });
                }
            }

            for (int userIndex = 0; userIndex < MAX_USERS_QUANTITY / 2; userIndex++)
            {
                var shipper = userFaker.Generate();
                var shipperResult = await _userManager.CreateAsync(shipper, "Shipper@123");
                if (shipperResult.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(shipper.Email);
                    await _userManager.AddToRolesAsync(user, new List<string> { EUserRole.SHIPPER.GetDisplayName() });
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedFunctions()
    {
        if (!_context.Functions.Any())
        {
            _context.Functions.AddRange(new List<Function>
            {
                new Function
                {
                    Id = EFunctionCode.DASHBOARD.GetDisplayName(), Name = "Dashboard", ParentId = null, SortOrder = 0,
                    Url = "/dashboard"
                },

                new Function
                {
                    Id = EFunctionCode.CONTENT.GetDisplayName(), Name = "Contents", ParentId = null, SortOrder = 0,
                    Url = "/content"
                },

                new Function
                {
                    Id = EFunctionCode.CONTENT_CATEGORY.GetDisplayName(), Name = "Categories",
                    ParentId = EFunctionCode.CONTENT.GetDisplayName(), SortOrder = 1, Url = "/content/category"
                },
                new Function
                {
                    Id = EFunctionCode.CONTENT_EVENT.GetDisplayName(), Name = "Events",
                    ParentId = EFunctionCode.CONTENT.GetDisplayName(), SortOrder = 1, Url = "/content/event"
                },
                new Function
                {
                    Id = EFunctionCode.CONTENT_REVIEW.GetDisplayName(), Name = "Reviews",
                    ParentId = EFunctionCode.CONTENT.GetDisplayName(), SortOrder = 2, Url = "/content/review"
                },
                new Function
                {
                    Id = EFunctionCode.CONTENT_TICKET.GetDisplayName(), Name = "Tickets",
                    ParentId = EFunctionCode.CONTENT.GetDisplayName(), SortOrder = 2, Url = "/content/ticket"
                },
                new Function
                {
                    Id = EFunctionCode.CONTENT_CHAT.GetDisplayName(), Name = "Chats",
                    ParentId = EFunctionCode.CONTENT.GetDisplayName(), SortOrder = 2, Url = "/content/chat"
                },
                new Function
                {
                    Id = EFunctionCode.CONTENT_PAYMENT.GetDisplayName(), Name = "Payments",
                    ParentId = EFunctionCode.CONTENT.GetDisplayName(), SortOrder = 2, Url = "/content/payment"
                },

                new Function
                {
                    Id = EFunctionCode.STATISTIC.GetDisplayName(), Name = "Statistics", ParentId = null, SortOrder = 0,
                    Url = "/statistic"
                },

                new Function
                {
                    Id = EFunctionCode.SYSTEM.GetDisplayName(), Name = "System", ParentId = null, SortOrder = 0,
                    Url = "/system"
                },

                new Function
                {
                    Id = EFunctionCode.SYSTEM_USER.GetDisplayName(), Name = "Users",
                    ParentId = EFunctionCode.SYSTEM.GetDisplayName(), SortOrder = 1, Url = "/system/user"
                },
                new Function
                {
                    Id = EFunctionCode.SYSTEM_ROLE.GetDisplayName(), Name = "Roles",
                    ParentId = EFunctionCode.SYSTEM.GetDisplayName(), SortOrder = 1, Url = "/system/role"
                },
                new Function
                {
                    Id = EFunctionCode.SYSTEM_FUNCTION.GetDisplayName(), Name = "Functions",
                    ParentId = EFunctionCode.SYSTEM.GetDisplayName(), SortOrder = 1, Url = "/system/function"
                },
                new Function
                {
                    Id = EFunctionCode.SYSTEM_PERMISSION.GetDisplayName(), Name = "Permissions",
                    ParentId = EFunctionCode.SYSTEM.GetDisplayName(), SortOrder = 1, Url = "/system/permission"
                },
            });

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedCommands()
    {
        if (!_context.Commands.Any())
        {
            _context.Commands.AddRange(new List<Command>()
            {
                new Command() { Id = "VIEW", Name = "View" },
                new Command() { Id = "CREATE", Name = "Create" },
                new Command() { Id = "UPDATE", Name = "Update" },
                new Command() { Id = "DELETE", Name = "Delete" },
                new Command() { Id = "APPROVE", Name = "Approve" },
            });

            await _context.SaveChangesAsync();
        }

        if (!_context.CommandInFunctions.Any())
        {
            var functions = _context.Functions;

            foreach (var function in functions)
            {
                var createAction = new CommandInFunction()
                {
                    CommandId = "CREATE",
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(createAction);

                var updateAction = new CommandInFunction()
                {
                    CommandId = "UPDATE",
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(updateAction);
                var deleteAction = new CommandInFunction()
                {
                    CommandId = "DELETE",
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(deleteAction);

                var viewAction = new CommandInFunction()
                {
                    CommandId = "VIEW",
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(viewAction);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedPermission()
    {
        if (!_context.Permissions.Any())
        {
            var functions = _context.Functions;
            var adminRole = await _roleManager.FindByNameAsync(EUserRole.ADMIN.GetDisplayName());
            foreach (var function in functions)
            {
                _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "CREATE"));
                _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "UPDATE"));
                _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "DELETE"));
                _context.Permissions.Add(new Permission(function.Id, adminRole.Id, "VIEW"));
            }

            var customerRole = await _roleManager.FindByNameAsync(EUserRole.CUSTOMER.GetDisplayName());
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT.GetDisplayName(), customerRole.Id, "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT.GetDisplayName(), customerRole.Id, "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT.GetDisplayName(), customerRole.Id, "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT.GetDisplayName(), customerRole.Id, "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CATEGORY.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_EVENT.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_PAYMENT.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_PAYMENT.GetDisplayName(), customerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_PAYMENT.GetDisplayName(), customerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_PAYMENT.GetDisplayName(), customerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_TICKET.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_REVIEW.GetDisplayName(), customerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_REVIEW.GetDisplayName(), customerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_REVIEW.GetDisplayName(), customerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_REVIEW.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CHAT.GetDisplayName(), customerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CHAT.GetDisplayName(), customerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CHAT.GetDisplayName(), customerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CHAT.GetDisplayName(), customerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM.GetDisplayName(), customerRole.Id, "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM.GetDisplayName(), customerRole.Id, "UPDATE"));
            _context.Permissions.Add(
                new Permission(EFunctionCode.SYSTEM_USER.GetDisplayName(), customerRole.Id, "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM_USER.GetDisplayName(), customerRole.Id,
                "UPDATE"));

            var organizerRole = await _roleManager.FindByNameAsync(EUserRole.SHIPPER.GetDisplayName());
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT.GetDisplayName(), organizerRole.Id, "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CATEGORY.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_EVENT.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_EVENT.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_EVENT.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_EVENT.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_PAYMENT.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_PAYMENT.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_PAYMENT.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_PAYMENT.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_TICKET.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_TICKET.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_TICKET.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_TICKET.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_REVIEW.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_REVIEW.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CHAT.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CHAT.GetDisplayName(), organizerRole.Id,
                "UPDATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CHAT.GetDisplayName(), organizerRole.Id,
                "CREATE"));
            _context.Permissions.Add(new Permission(EFunctionCode.CONTENT_CHAT.GetDisplayName(), organizerRole.Id,
                "DELETE"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM_USER.GetDisplayName(), organizerRole.Id,
                "VIEW"));
            _context.Permissions.Add(new Permission(EFunctionCode.SYSTEM_USER.GetDisplayName(), organizerRole.Id,
                "UPDATE"));

            await _context.SaveChangesAsync();
        }
    }
}