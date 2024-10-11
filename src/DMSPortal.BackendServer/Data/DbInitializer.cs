using System.Globalization;
using Bogus;
using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;

namespace DMSPortal.BackendServer.Data;

public class DbInitializer
{
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
        await SeedRoles();
        await SeedUsers();
        await SeedFunctions();
        await SeedCommands();
        await SeedPermissions();
        await SeedPitchGroups();
        await SeedBranches();
        await SeedPitches();
        await SeedClasses();
        await SeedStudents();
        await SeedShifts();
        await SeedClassInShifts();
        await SeedAttendances();
        await SeedNotes();
    }

    private async Task SeedRoles()
    {
        if (!_roleManager.Roles.Any())
        {
            await _roleManager.CreateAsync(new Role()
            {
                Name = nameof(EUserRole.ADMIN),
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = nameof(EUserRole.VAN_PHONG)
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = nameof(EUserRole.KE_TOAN),
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = nameof(EUserRole.HANH_CHINH),
            });
            await _roleManager.CreateAsync(new Role()
            {
                Name = nameof(EUserRole.GIAO_VIEN),
            });
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedUsers()
    {
        if (!_userManager.Users.Any())
        {
            #region Generate Admin

            var admin = new User()
            {
                UserName = "admin",
                FullName = "Admin",
                Email = "admin@gmail.com",
                PhoneNumber = new Faker("vi").Phone.PhoneNumber("####-###-###"),
                Dob = new Faker("vi").Person.DateOfBirth.ToUniversalTime(),
                Gender = EGender.MALE,
                Avatar = new Faker("vi").Internet.Avatar(),
                Address = new Faker("vi").Address.ToString(),
                Status = EUserStatus.ACTIVE,
            };

            var result = await _userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync("admin@gmail.com");
                if (user != null)
                    await _userManager.AddToRolesAsync(user,
                        new List<string>
                        {
                            nameof(EUserRole.ADMIN), nameof(EUserRole.KE_TOAN), nameof(EUserRole.VAN_PHONG),
                            nameof(EUserRole.HANH_CHINH)
                        });
            }

            #endregion

            #region Generate HanhChinh

            var hanhChinhFaker = new Faker<User>("vi")
                .RuleFor(u => u.UserName, f => f.Person.UserName)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("####-###-###"))
                .RuleFor(u => u.Dob, f => f.Person.DateOfBirth.ToUniversalTime())
                .RuleFor(u => u.Gender, f => f.PickRandom<EGender>())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Address, f => f.Address.ToString())
                .RuleFor(u => u.NumberOfBranches, _ => 2)
                .RuleFor(u => u.Status, _ => EUserStatus.ACTIVE);

            for (var userIndex = 0; userIndex < 10; userIndex++)
            {
                var hanhchinh = hanhChinhFaker.Generate();
                var hanhchinhResult = await _userManager.CreateAsync(hanhchinh, "User@123");
                if (!hanhchinhResult.Succeeded) continue;

                var user = await _userManager.FindByEmailAsync(hanhchinh?.Email ?? "");
                if (user != null)
                    await _userManager.AddToRoleAsync(user, nameof(EUserRole.HANH_CHINH));
            }

            #endregion

            #region Generate GiaoVien

            var giaoVienFaker = new Faker<User>("vi")
                .RuleFor(u => u.UserName, f => f.Person.UserName)
                .RuleFor(u => u.FullName, f => f.Person.FullName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber("####-###-###"))
                .RuleFor(u => u.Dob, f => f.Person.DateOfBirth.ToUniversalTime())
                .RuleFor(u => u.Gender, f => f.PickRandom<EGender>())
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.Address, f => f.Address.ToString())
                .RuleFor(u => u.NumberOfBranches, _ => 2)
                .RuleFor(u => u.Status, _ => EUserStatus.ACTIVE);

            for (var userIndex = 0; userIndex < 10; userIndex++)
            {
                var giaovien = giaoVienFaker.Generate();
                var giaovienResult = await _userManager.CreateAsync(giaovien, "User@123");
                if (!giaovienResult.Succeeded) continue;

                var user = await _userManager.FindByEmailAsync(giaovien?.Email ?? "");
                if (user != null)
                    await _userManager.AddToRoleAsync(user, nameof(EUserRole.GIAO_VIEN));
            }

            #endregion

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedFunctions()
    {
        if (!_context.Functions.Any())
        {
            _context.Functions.AddRange(new List<Function>
            {
                #region Main Functions

                new Function
                {
                    Id = nameof(EFunctionCode.GENERAL), Name = "Generals", ParentId = null, SortOrder = 0,
                    Url = "/general"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.STATISTIC), Name = "Statistics", ParentId = null, SortOrder = 0,
                    Url = "/statistic"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.SYSTEM), Name = "Systems", ParentId = null, SortOrder = 0, Url = "/system"
                },

                #endregion

                #region General Functions

                new Function
                {
                    Id = nameof(EFunctionCode.GENERAL_ATTENDANCE),
                    Name = "Attendances",
                    ParentId = nameof(EFunctionCode.GENERAL),
                    SortOrder = 1,
                    Url = "/general/attendance"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.GENERAL_BRANCH),
                    Name = "Branches",
                    ParentId = nameof(EFunctionCode.GENERAL),
                    SortOrder = 1,
                    Url = "/general/branch"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.GENERAL_CLASS),
                    Name = "Classes",
                    ParentId = nameof(EFunctionCode.GENERAL),
                    SortOrder = 1,
                    Url = "/general/class"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.GENERAL_NOTE),
                    Name = "Notes",
                    ParentId = nameof(EFunctionCode.GENERAL),
                    SortOrder = 1,
                    Url = "/general/note"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.GENERAL_PITCH),
                    Name = "Pitches",
                    ParentId = nameof(EFunctionCode.GENERAL),
                    SortOrder = 1,
                    Url = "/general/pitch"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.GENERAL_PITCH_GROUP),
                    Name = "Pitch Groups",
                    ParentId = nameof(EFunctionCode.GENERAL),
                    SortOrder = 1,
                    Url = "/general/pitch-group"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.GENERAL_SHIFT),
                    Name = "Shifts",
                    ParentId = nameof(EFunctionCode.GENERAL),
                    SortOrder = 1,
                    Url = "/general/shift"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.GENERAL_STUDENT),
                    Name = "Students",
                    ParentId = nameof(EFunctionCode.GENERAL),
                    SortOrder = 1,
                    Url = "/general/student"
                },

                #endregion

                #region System Functions

                new Function
                {
                    Id = nameof(EFunctionCode.SYSTEM_USER), Name = "Users",
                    ParentId = nameof(EFunctionCode.SYSTEM), SortOrder = 1, Url = "/system/user"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.SYSTEM_ROLE), Name = "Roles",
                    ParentId = nameof(EFunctionCode.SYSTEM), SortOrder = 1, Url = "/system/role"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.SYSTEM_FUNCTION), Name = "Functions",
                    ParentId = nameof(EFunctionCode.SYSTEM), SortOrder = 1, Url = "/system/function"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.SYSTEM_COMMAND), Name = "Commands",
                    ParentId = nameof(EFunctionCode.SYSTEM), SortOrder = 1, Url = "/system/command"
                },
                new Function
                {
                    Id = nameof(EFunctionCode.SYSTEM_PERMISSION), Name = "Permissions",
                    ParentId = nameof(EFunctionCode.SYSTEM), SortOrder = 1, Url = "/system/permission"
                },

                #endregion
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
                new Command() { Id = nameof(ECommandCode.VIEW), Name = "View" },
                new Command() { Id = nameof(ECommandCode.CREATE), Name = "Create" },
                new Command() { Id = nameof(ECommandCode.UPDATE), Name = "Update" },
                new Command() { Id = nameof(ECommandCode.DELETE), Name = "Delete" },
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
                    CommandId = nameof(ECommandCode.CREATE),
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(createAction);

                var updateAction = new CommandInFunction()
                {
                    CommandId = nameof(ECommandCode.UPDATE),
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(updateAction);
                var deleteAction = new CommandInFunction()
                {
                    CommandId = nameof(ECommandCode.DELETE),
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(deleteAction);

                var viewAction = new CommandInFunction()
                {
                    CommandId = nameof(ECommandCode.VIEW),
                    FunctionId = function.Id
                };
                _context.CommandInFunctions.Add(viewAction);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedPermissions()
    {
        if (!_context.Permissions.Any())
        {
            var functions = _context.Functions;
            var adminRole = await _roleManager.FindByNameAsync(nameof(EUserRole.ADMIN));
            if (adminRole != null)
            {
                foreach (var function in functions)
                {
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, nameof(ECommandCode.CREATE)));
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, nameof(ECommandCode.VIEW)));
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, nameof(ECommandCode.UPDATE)));
                    _context.Permissions.Add(new Permission(function.Id, adminRole.Id, nameof(ECommandCode.DELETE)));
                }
            }

            var hanhchinhRole = await _roleManager.FindByNameAsync(nameof(EUserRole.HANH_CHINH));
            if (hanhchinhRole != null)
            {
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL), hanhchinhRole.Id,
                    nameof(ECommandCode.CREATE)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL), hanhchinhRole.Id,
                    nameof(ECommandCode.DELETE)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_ATTENDANCE), hanhchinhRole.Id,
                    nameof(ECommandCode.CREATE)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_ATTENDANCE), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_ATTENDANCE), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_BRANCH), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_CLASS), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_CLASS), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_NOTE), hanhchinhRole.Id,
                    nameof(ECommandCode.CREATE)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_NOTE), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_NOTE), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_NOTE), hanhchinhRole.Id,
                    nameof(ECommandCode.DELETE)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_PITCH), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_PITCH), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_PITCH_GROUP), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_PITCH_GROUP), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_SHIFT), hanhchinhRole.Id,
                    nameof(ECommandCode.CREATE)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_SHIFT), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_SHIFT), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_STUDENT), hanhchinhRole.Id,
                    nameof(ECommandCode.CREATE)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_STUDENT), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_STUDENT), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.GENERAL_STUDENT), hanhchinhRole.Id,
                    nameof(ECommandCode.DELETE)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.SYSTEM), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.SYSTEM), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));

                _context.Permissions.Add(new Permission(nameof(EFunctionCode.SYSTEM_USER), hanhchinhRole.Id,
                    nameof(ECommandCode.VIEW)));
                _context.Permissions.Add(new Permission(nameof(EFunctionCode.SYSTEM_USER), hanhchinhRole.Id,
                    nameof(ECommandCode.UPDATE)));
            }


            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedPitchGroups()
    {
        if (!_context.PitchGroups.Any())
        {
            for (var i = 1; i <= 10; i++)
            {
                var pitchGroupFaker = new Faker<PitchGroup>("vi")
                    .RuleFor(x => x.Name, _ => $"Cơ sở {i}")
                    .RuleFor(x => x.NumberOfBranches, _ => 2)
                    .RuleFor(x => x.Status, f => f.PickRandom<EPitchGroupStatus>());

                var pitchGroup = pitchGroupFaker.Generate();
                await _context.PitchGroups.AddAsync(pitchGroup);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedBranches()
    {
        if (!_context.Branches.Any())
        {
            var pitchGroups = await _context.PitchGroups.AsNoTracking().ToArrayAsync();
            var usersInHanhChinh = await _userManager.GetUsersInRoleAsync(nameof(EUserRole.HANH_CHINH));
            var users = usersInHanhChinh.ToArray();

            for (var i = 0; i < 20; i++)
            {
                var user = users[(int)(i / 2)];
                var pitchGroup = pitchGroups[(int)(i / 2)];

                var branchFaker = new Faker<Branch>("vi")
                    .RuleFor(x => x.Name, f => f.Address.State())
                    .RuleFor(x => x.Address, (f, x) => x.Name)
                    .RuleFor(x => x.ManagerId, _ => user.Id)
                    .RuleFor(x => x.PitchGroupId, _ => pitchGroup.Id)
                    .RuleFor(x => x.NumberOfPitches, _ => 4)
                    .RuleFor(x => x.Status, f => f.PickRandom<EBranchStatus>());

                var branch = branchFaker.Generate();
                await _context.Branches.AddAsync(branch);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedPitches()
    {
        if (!_context.Pitches.Any())
        {
            var branches = await _context.Branches.AsNoTracking().ToArrayAsync();

            for (var i = 0; i < 80; i++)
            {
                var branch = branches[(int)(i / 4)];

                var pitchFaker = new Faker<Pitch>("vi")
                    .RuleFor(x => x.Name, f => f.Address.State())
                    .RuleFor(x => x.BranchId, _ => branch.Id)
                    .RuleFor(x => x.NumberOfClasses, _ => 4)
                    .RuleFor(x => x.Status, f => f.PickRandom<EPitchStatus>());

                var pitch = pitchFaker.Generate();
                await _context.Pitches.AddAsync(pitch);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedClasses()
    {
        if (!_context.Classes.Any())
        {
            var pitches = await _context.Pitches
                .AsNoTracking()
                .ToArrayAsync();

            var users = (await _userManager.GetUsersInRoleAsync(nameof(EUserRole.GIAO_VIEN))).ToArray();

            for (int i = 0; i < 100; i++)
            {
                var pitch = pitches[new Faker().Random.Int(0, pitches.Length - 1)];
                var user = users[new Faker().Random.Int(0, users.Length - 1)];
                var classFaker = new Faker<Class>("vi")
                    .RuleFor(x => x.Name, f => $"{f.PickRandom<EClassType>().GetDisplayName()}-{i:00}")
                    .RuleFor(x => x.PitchId, f => pitch.Id)
                    .RuleFor(x => x.TeacherId, f => user.Id)
                    .RuleFor(x => x.Status, f => f.PickRandom<EClassStatus>());

                var generatedClass = classFaker.Generate();
                await _context.Classes.AddAsync(generatedClass);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedStudents()
    {
        if (!_context.Students.Any())
        {
            var classes = await _context.Classes
                .AsNoTracking()
                .ToArrayAsync();

            var studentFaker = new Faker<Student>("vi")
                .RuleFor(x => x.FullName, f => f.Person.FullName)
                .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumber("####-###-###"))
                .RuleFor(x => x.DOB, f => f.Person.DateOfBirth.ToUniversalTime())
                .RuleFor(x => x.Address, f => f.Address.ToString())
                .RuleFor(x => x.Gender, f => f.PickRandom<EGender>())
                .RuleFor(x => x.Height, f => f.Random.Int(100, 180))
                .RuleFor(x => x.Weight, f => f.Random.Int(30, 60))
                .RuleFor(x => x.FavouritePosition,
                    f => f.PickRandom<string>("Thủ môn", "Hậu vệ", "Trung vệ", "Tiền vệ", "Tiền đạo"))
                .RuleFor(x => x.FatherFullName, f => f.Person.FullName)
                .RuleFor(x => x.FatherBirthYear, f => f.Random.Int(1970, 1990))
                .RuleFor(x => x.FatherAddress, f => f.Address.ToString())
                .RuleFor(x => x.FatherPhoneNumber, f => f.Phone.PhoneNumber("####-###-###"))
                .RuleFor(x => x.FatherEmail, f => f.Person.Email)
                .RuleFor(x => x.MotherFullName, f => f.Person.FullName)
                .RuleFor(x => x.MotherBirthYear, f => f.Random.Int(1970, 1990))
                .RuleFor(x => x.MotherAddress, f => f.Address.ToString())
                .RuleFor(x => x.MotherPhoneNumber, f => f.Phone.PhoneNumber("####-###-###"))
                .RuleFor(x => x.MotherEmail, f => f.Person.Email)
                .RuleFor(x => x.Status, f => f.PickRandom<EStudentStatus>());

            var students = studentFaker.Generate(2000);
            await _context.Students.AddRangeAsync(students);

            await _context.SaveChangesAsync();

            foreach (var student in students)
            {
                var joinedClass = new Faker("vi").PickRandom<Class>(classes);

                joinedClass.NumberOfStudents++;
                student.NumberOfClasses++;

                var studentInClassFaker = new Faker<StudentInClass>()
                    .RuleFor(x => x.StudentId, _ => student.Id)
                    .RuleFor(x => x.ClassId, _ => joinedClass.Id)
                    .RuleFor(x => x.ClassType, f => f.PickRandom<EClassType>())
                    .RuleFor(x => x.PaymentStatus, f => f.PickRandom<EPaymentStatus>())
                    .RuleFor(x => x.JoinedAt, f => f.Date.PastOffset().UtcDateTime)
                    .RuleFor(x => x.Status, f => f.PickRandom<EStudentInClassStatus>());

                var studentInClass = studentInClassFaker.Generate();
                await _context.StudentInClasses.AddAsync(studentInClass);
                _context.Students.Update(student);
                _context.Classes.Update(joinedClass);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedShifts()
    {
        if (!_context.Shifts.Any())
        {
            var excuteDate = DateTime.ParseExact("07/04/2024", "dd/MM/yyyy", CultureInfo.InvariantCulture)
                .Subtract(TimeSpan.FromDays(100));

            while (excuteDate < DateTime.UtcNow)
            {
                var shifts = new List<Shift>
                {
                    new Shift
                    {
                        Name = $"Ca 1",
                        Date = excuteDate.ToString("dd/MM/yyyy"),
                        StartTime = "07:00",
                        EndTime = "09:00",
                    },
                    new Shift
                    {
                        Name = $"Ca 2",
                        Date = excuteDate.ToString("dd/MM/yyyy"),
                        StartTime = "09:30",
                        EndTime = "11:30",
                    },
                    new Shift
                    {
                        Name = $"Ca 3",
                        Date = excuteDate.ToString("dd/MM/yyyy"),
                        StartTime = "13:00",
                        EndTime = "15:00",
                    },
                    new Shift
                    {
                        Name = $"Ca 4",
                        Date = excuteDate.ToString("dd/MM/yyyy"),
                        StartTime = "15:30",
                        EndTime = "17:30",
                    }
                };
                await _context.Shifts.AddRangeAsync(shifts);

                excuteDate = excuteDate.AddDays(1);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedClassInShifts()
    {
        if (!_context.ClassInShifts.Any())
        {
            var classes = await _context.Classes.AsNoTracking().ToArrayAsync();
            var shifts = await _context.Shifts.AsNoTracking().ToArrayAsync();

            foreach (var pickedClass in classes)
            {
                var shift = new Faker("vi").PickRandom<Shift>(shifts);

                var exist = _context.ClassInShifts
                    .AsNoTracking()
                    .Include(x => x.Class)
                    .Any(x => x.Class.PitchId.Equals(pickedClass.PitchId) && x.ShiftId.Equals(shift.Id));

                if (exist) continue;

                var classInShift = new ClassInShift
                {
                    ClassId = pickedClass.Id,
                    ShiftId = shift.Id
                };

                await _context.ClassInShifts.AddAsync(classInShift);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedAttendances()
    {
        if (!_context.Attendances.Any())
        {
            var classInShifts = await _context.ClassInShifts
                .AsNoTracking()
                .Include(x => x.Shift)
                .ToArrayAsync();

            foreach (var classInShift in classInShifts)
            {
                var students = await _context.StudentInClasses
                    .AsNoTracking()
                    .Where(x => x.ClassId.Equals(classInShift.ClassId))
                    .Include(x => x.Student)
                    .Select(x => x.Student)
                    .ToArrayAsync();

                var attendedStudents = new Faker("vi")
                    .PickRandom<Student>(students, int.Min(10, students.Length))
                    .Select(x => new Attendance
                    {
                        ClassId = classInShift.ClassId,
                        ShiftId = classInShift.ShiftId,
                        StudentId = x.Id,
                        CheckinAt = DateTime.ParseExact(
                            $"{classInShift.Shift.Date} {classInShift.Shift.StartTime}", "dd/MM/yyyy HH:mm",
                            CultureInfo.InvariantCulture).ToUniversalTime(),
                        CheckoutAt = DateTime.ParseExact(
                            $"{classInShift.Shift.Date} {classInShift.Shift.EndTime}", "dd/MM/yyyy HH:mm",
                            CultureInfo.InvariantCulture).ToUniversalTime()
                    })
                    .ToList();

                await _context.Attendances.AddRangeAsync(attendedStudents);
            }

            await _context.SaveChangesAsync();
        }
    }

    private async Task SeedNotes()
    {
        if (!_context.Notes.Any())
        {
            var students = await _context.Students.AsNoTracking().ToArrayAsync();

            foreach (var student in students)
            {
                var noteFaker = new Faker<Note>("vi")
                    .RuleFor(x => x.Content, f => f.Lorem.ToString())
                    .RuleFor(x => x.StudentId, _ => student.Id);

                var notes = noteFaker.Generate(2);
                await _context.Notes.AddRangeAsync(notes);
            }

            await _context.SaveChangesAsync();
        }
    }
}