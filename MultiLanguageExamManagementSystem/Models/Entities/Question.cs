namespace MultiLanguageExamManagementSystem.Models.Entities;

public class Question
{
    public string QuestionId { get; set; } = Guid.NewGuid().ToString();
    public string Text { get; set; }
    public string Answer { get; set; }
    public int DifficultyLevel { get; set; }

    public ICollection<ExamQuestion> ExamQuestions { get; set; }
}