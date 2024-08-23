using DMSPortal.BackendServer.Data.Entities;
using DMSPortal.BackendServer.Data.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DMSPortal.BackendServer.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        IEnumerable<EntityEntry> modified = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified or EntityState.Added or EntityState.Deleted);
        
        foreach (var item in modified)
        {
            if (item.Entity is IDateTracking changedOrAddedItem)
            {
                if (item.State == EntityState.Added)
                {
                    changedOrAddedItem.CreatedAt = DateTime.UtcNow;
                }
                else
                {
                    changedOrAddedItem.UpdatedAt = DateTime.UtcNow;
                }
            }

            if (item.Entity is ISoftDeletable deletedEntity)
            {
                if (item.State == EntityState.Deleted)
                {
                    item.State = EntityState.Modified;
                    item.Property(nameof(ISoftDeletable.IsDeleted)).CurrentValue = true;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Attendance>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Branch>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Class>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Command>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Function>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Note>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Pitch>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<PitchGroup>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Role>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Shift>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Student>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<User>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);


        #region One-to-many
        
        builder.Entity<Branch>()
            .HasOne(x => x.Manager)
            .WithMany(x => x.Branches)
            .HasForeignKey(x => x.ManagerId);
        builder.Entity<Branch>()
            .HasOne(x => x.PitchGroup)
            .WithMany(x => x.Branches)
            .HasForeignKey(x => x.PitchGroupId);
        
        builder.Entity<Class>()
            .HasOne(x => x.Pitch)
            .WithMany(x => x.Classes)
            .HasForeignKey(x => x.PitchId);
        
        builder.Entity<Class>()
            .HasOne(x => x.Pitch)
            .WithMany(x => x.Classes)
            .HasForeignKey(x => x.PitchId);
        
        builder.Entity<Note>()
            .HasOne(x => x.Student)
            .WithMany(x => x.Notes)
            .HasForeignKey(x => x.StudentId);
        
        builder.Entity<Pitch>()
            .HasOne(x => x.Branch)
            .WithMany(x => x.Pitches)
            .HasForeignKey(x => x.BranchId);
        
        #endregion
        
        #region Many-to-many
        
        builder.Entity<Attendance>()
            .HasOne(x => x.Class)
            .WithMany(x => x.Attendances)
            .HasForeignKey(x => x.ClassId);
        builder.Entity<Attendance>()
            .HasOne(x => x.Shift)
            .WithMany(x => x.Attendances)
            .HasForeignKey(x => x.ShiftId);
        builder.Entity<Attendance>()
            .HasOne(x => x.Student)
            .WithMany(x => x.Attendances)
            .HasForeignKey(x => x.StudentId);
        
        builder.Entity<ClassInShift>()
            .HasOne(x => x.Class)
            .WithMany(x => x.ClassInShifts)
            .HasForeignKey(x => x.ClassId);
        builder.Entity<ClassInShift>()
            .HasOne(x => x.Shift)
            .WithMany(x => x.ClassInShifts)
            .HasForeignKey(x => x.ShiftId);
        
        builder.Entity<CommandInFunction>()
            .HasOne(x => x.Function)
            .WithMany(x => x.CommandInFunctions)
            .HasForeignKey(x => x.FunctionId);
        builder.Entity<CommandInFunction>()
            .HasOne(x => x.Command)
            .WithMany(x => x.CommandInFunctions)
            .HasForeignKey(x => x.CommandId);

        builder.Entity<Permission>()
            .HasOne(x => x.Function)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.FunctionId);
        builder.Entity<Permission>()
            .HasOne(x => x.Role)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.RoleId);
        builder.Entity<Permission>()
            .HasOne(x => x.Command)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.CommandId);
        
        builder.Entity<StudentInClass>()
            .HasOne(x => x.Student)
            .WithMany(x => x.StudentInClasses)
            .HasForeignKey(x => x.StudentId);
        builder.Entity<StudentInClass>()
            .HasOne(x => x.Class)
            .WithMany(x => x.StudentInClasses)
            .HasForeignKey(x => x.ClassId);

        #endregion
    }

    public required DbSet<Attendance> Attendances { set; get; }
    public required DbSet<Branch> Branches { set; get; }
    public required DbSet<Class> Classes { set; get; }
    public required DbSet<ClassInShift> ClassInShifts { set; get; }
    public required DbSet<Command> Commands { set; get; }
    public required DbSet<CommandInFunction> CommandInFunctions { set; get; }
    public required DbSet<Function> Functions { set; get; }
    public required DbSet<Note> Notes { set; get; }
    public required DbSet<Permission> Permissions { set; get; }
    public required DbSet<Pitch> Pitches { set; get; }
    public required DbSet<PitchGroup> PitchGroups { set; get; }
    public required DbSet<Role> Roles { set; get; }
    public required DbSet<Shift> Shifts { set; get; }
    public required DbSet<Student> Students { set; get; }
    public required DbSet<StudentInClass> StudentInClasses { set; get; }
    public required DbSet<User> Users { set; get; }
}