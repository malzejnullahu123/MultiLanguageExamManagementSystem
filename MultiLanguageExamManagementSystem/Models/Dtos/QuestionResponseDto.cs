namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class QuestionResponseDto
{
    public string QuestionId { get; set; }
    public string Text { get; set; }
    public List<string> PossibleAnswers { get; set; }
}