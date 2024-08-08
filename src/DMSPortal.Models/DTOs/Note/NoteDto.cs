using DMSPortal.Models.DTOs.Student;

namespace DMSPortal.Models.DTOs.Note;

public class NoteDto
{
    public string Id { get; set; }
    
    public string Content { get; set; }
    
    public string StudentId { get; set; }
    
    public StudentDto? Student { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
}