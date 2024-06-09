using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class UserResponseDto
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Role { get; set; } // Professor or Student
    
    public ICollection<Exam> CreatedExams { get; set; }
    public ICollection<TakenExam> TakenExams { get; set; }
}