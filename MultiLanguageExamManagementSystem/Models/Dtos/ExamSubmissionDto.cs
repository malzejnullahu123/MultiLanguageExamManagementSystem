namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class ExamSubmissionDto
{
    public string ExamId { get; set; }
    public string UserId { get; set; }
    public Dictionary<string, string> Answers { get; set; }
}