using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DMSPortal.BackendServer.Data.EntityBases;
using DMSPortal.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DMSPortal.BackendServer.Data.Entities;

[Table("StudentInClasses")]
[PrimaryKey("ClassId", "StudentId")]
public class StudentInClass : EntityBase
{
    [Required]
    [MaxLength(50)]
    public string ClassId { get; set; }

    [Required]
    [MaxLength(50)]
    public string StudentId { get; set; }

    public DateTime JoinedAt { get; set; }

    [Range(0, Double.PositiveInfinity)]
    public int? NumberOfAttendance { get; set; } = 0;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EClassType ClassType { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EStudentInClassStatus Status { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EPaymentStatus PaymentStatus { get; set; }

    [ForeignKey("StudentId")]
    public virtual Student Student { get; set; }
    
    [ForeignKey("ClassId")]
    public virtual Class Class { get; set; }
}