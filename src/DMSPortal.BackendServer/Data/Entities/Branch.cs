using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DMSPortal.BackendServer.Abstractions.Entity;
using DMSPortal.Models.Enums;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Branches")]
public class Branch : IdentityEntityBase<string>
{
    [Required]
    [Column(TypeName = "text")]
    public string Name { get; set; }

    [Column(TypeName = "text")]
    public string Address { get; set; }

    [Range(0, Double.PositiveInfinity)]
    public int? NumberOfPitches { get; set; } = 0;

    [Required]
    [MaxLength(50)]
    public string PitchGroupId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ManagerId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EBranchStatus Status { get; set; }

    [ForeignKey("PitchGroupId")]
    public virtual PitchGroup PitchGroup { get; set; }

    [ForeignKey("ManagerId")]
    public virtual User Manager { get; set; }

    public virtual ICollection<Pitch> Pitches { get; set; } = new List<Pitch>();
}