namespace MultiLanguageExamManagementSystem.Models.Entities;

public class ExamRequest
{
    public string ExamRequestId { get; set; } = Guid.NewGuid().ToString();
    public string ExamId { get; set; }
    public Exam Exam { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public bool IsApproved { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.UtcNow;
}