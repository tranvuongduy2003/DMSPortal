using DMSPortal.Models.DTOs.Student;

namespace DMSPortal.Models.DTOs.Note;

public class NoteDto
{
    public string Id { get; set; }
    
    public string Content { get; set; }
    
    public string StudentId { get; set; }
    
    public StudentDto? Student { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? DeletedAt { get; set; }
}