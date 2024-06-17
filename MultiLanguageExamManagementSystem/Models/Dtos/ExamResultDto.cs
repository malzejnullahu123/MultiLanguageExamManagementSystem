namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class ExamResultDto
{
    public string ExamId { get; set; }
    public string UserId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double Score { get; set; }
}