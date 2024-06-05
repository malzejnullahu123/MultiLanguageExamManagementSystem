using LifeEcommerce.Helpers;
using Microsoft.Extensions.Localization;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Services.IServices
{
    public interface ICultureService
    {
        // Your code here
        // methods for string localization, languages and localization resources
        LocalizationResource this [string location] { get; }

        LocalizationResource GetString(string location);

        Task<List<LanguagesResponse>> GetLanguage;
        Task<LanguagesResponse> GetLanguageById(int id);
        

    }
}
