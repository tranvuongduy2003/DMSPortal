using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Roles")]
public class Role : IdentityRole
{
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}