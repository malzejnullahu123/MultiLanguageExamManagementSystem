namespace MultiLanguageExamManagementSystem.Models.Entities;

public class ExamQuestion
{
    public string ExamId { get; set; } = Guid.NewGuid().ToString();
    public Exam Exam { get; set; }

    public string QuestionId { get; set; }
    public Question Question { get; set; }
}