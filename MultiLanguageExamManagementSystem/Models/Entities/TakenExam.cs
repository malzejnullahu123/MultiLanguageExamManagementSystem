namespace MultiLanguageExamManagementSystem.Models.Entities;

public class TakenExam
{
    public string TakenExamId { get; set; }
    public string ExamId { get; set; }
    public Exam Exam { get; set; }

    public string StudentId { get; set; }
    public User Student { get; set; }

    public DateTime TakenOn { get; set; }
    public bool IsCompleted { get; set; }
}