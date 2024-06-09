namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class ExamResponseDto
{
    public string ExamId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CreatorId { get; set; }
    public string CreatorName { get; set; }
}