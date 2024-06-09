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
        
        //String localization
        LocalizationResource this [string location] { get; }
        LocalizationResource GetString(string location);
        string GetLocator(string @namespace, string key);

        //Language
        Task<List<LanguagesResponseDto>> GetAllLanguages();
        Task<LanguagesResponseDto> GetByIdLanguage(int id);
        Task CreateLanguage(LanguageRequestDto languageRequestDto);
        Task UpdateLanguage(int id, LanguageRequestDto languageRequestDto);
        Task DeleteLanguage(int id);
        
        //Localization Resources
        Task<List<LocalizationResourceResponseDto>> GetAllLocalizationResources();
        Task<LocalizationResourceResponseDto> GetLocalizationResourceById(int id);
        Task<List<LocalizationResourceResponseDto>> GetLocalizationResourcesByLanguageId(int languageId);
        Task CreateLocalizationResource(LocalizationResourceRequestDto localizationResourceRequestDto);
        Task UpdateLocalizationResource(int id, LocalizationResourceRequestDto localizationResourceRequestDto);
        Task DeleteLocalizationResource(string @namespace, string key);
        

    }
}
