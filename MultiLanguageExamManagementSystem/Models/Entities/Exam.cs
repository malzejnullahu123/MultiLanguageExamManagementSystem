namespace MultiLanguageExamManagementSystem.Models.Entities;

public class Exam
{
    public string ExamId { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatorId { get; set; }
    public User Creator { get; set; }

    public ICollection<ExamQuestion> ExamQuestions { get; set; }
    public ICollection<TakenExam> TakenExams { get; set; }
}