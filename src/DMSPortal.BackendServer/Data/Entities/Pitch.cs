using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DMSPortal.BackendServer.Abstractions.Entity;
using DMSPortal.Models.Enums;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("Pitches")]
public class Pitch : IdentityEntityBase<string>
{
    [Required]
    [Column(TypeName = "text")]
    public string Name { get; set; }

    [Required]
    [MaxLength(50)] 
    public string BranchId { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EPitchStatus Status { get; set; }

    [Range(0, Double.PositiveInfinity)]
    public int? NumberOfClasses { get; set; } = 0;

    [ForeignKey("BranchId")]
    public virtual Branch Branch { get; set; }
    
    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}