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
            .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);
        foreach (EntityEntry item in modified)
        {
            if (item.Entity is IDateTracking changedOrAddedItem)
            {
                if (item.State == EntityState.Added)
                {
                    changedOrAddedItem.CreatedAt = DateTime.Now;
                }
                else
                {
                    changedOrAddedItem.UpdatedAt = DateTime.Now;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Role>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Command>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        builder.Entity<Function>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);


        #region Many-to-many

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

        #endregion
    }

    public DbSet<User> Users { set; get; }
    public DbSet<Role> Roles { set; get; }
    public DbSet<Command> Commands { set; get; }
    public DbSet<CommandInFunction> CommandInFunctions { set; get; }
    public DbSet<Function> Functions { set; get; }
    public DbSet<Permission> Permissions { set; get; }
}