using System.ComponentModel.DataAnnotations.Schema;

namespace MultiLanguageExamManagementSystem.Models.Entities
{
    public class Language
    {
        // Your code here
        // Language will have Id, Name, LanguageCode, CountryId (IsDefault and IsSelected are optional properties)
        public int Id { get; set; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public bool IsDefault { get; set; }
        public  bool IsSelected { get; set; }
    }
}
