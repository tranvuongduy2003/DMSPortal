using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DMSPortal.BackendServer.Data.EntityBases;
using DMSPortal.Models.Enums;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("PitchGroups")]
public class PitchGroup : IdentityEntityBase<string>
{
    [Required]
    [Column(TypeName = "text")]
    public string Name { get; set; }

    [Range(0, Double.PositiveInfinity)]
    public int? NumberOfBranches { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EPitchGroupStatus Status { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}