namespace MultiLanguageExamManagementSystem.Models.Dtos;

public class LanguageRequestDto
{
    public string Name { get; set; }
    public string LanguageCode { get; set; }
    public int CountryId { get; set; }
    // public Country Country { get; set; }
    public bool IsDefault { get; set; }
    public  bool IsSelected { get; set; }
}