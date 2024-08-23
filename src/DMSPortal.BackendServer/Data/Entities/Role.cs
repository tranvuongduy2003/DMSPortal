using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using DMSPortal.BackendServer.Data.Interfaces;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Roles")]
public class Role : IdentityRole, IDateTracking
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}