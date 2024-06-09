namespace MultiLanguageExamManagementSystem.Models.Entities;

public class User
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Professor or Student

    public ICollection<Exam> CreatedExams { get; set; }
    public ICollection<TakenExam> TakenExams { get; set; }
}