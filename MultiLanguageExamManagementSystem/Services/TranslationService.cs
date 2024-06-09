using System.Collections.Concurrent;
using System.Text;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Helpers;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;
using Newtonsoft.Json;

namespace MultiLanguageExamManagementSystem.Services
{
    public class TranslationService
    {
        
        // Your code here

        // You can make it static, add interface or whatever u want

        //https://www.deepl.com/pro-api?cta=header-pro-api - check the documentation
        
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly IUnitOfWork _unitOfWork;

        public TranslationService(HttpClient httpClient, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _httpClient = httpClient;
            _unitOfWork = unitOfWork;
            _apiKey = configuration["DeepLApiKey"];
    
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {_apiKey}");
        }


        public async Task TranslateForLanguage(Language newLanguage, List<LocalizationResource> localizationResources)
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
            var concurrentBag = new ConcurrentBag<LocalizationResource>();

            await Parallel.ForEachAsync(localizationResources, options, 
                async (localizationResource, cancellationToken) => await AddResourceToLanguage(newLanguage, localizationResource, concurrentBag));

            foreach (var localizationResource in concurrentBag)
            {
                _unitOfWork.Repository<LocalizationResource>().Create(localizationResource);
            }
            _unitOfWork.Complete();
        }

        public async Task TranslateForResource(LocalizationResource localizationResource, List<Language> languages)
        {
            var options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
            var concurrentBag = new ConcurrentBag<LocalizationResource>();

            await Parallel.ForEachAsync(languages, options, 
                async (language, cancellationToken) => await AddResourceToLanguage(language, localizationResource, concurrentBag));

            foreach (var localization in concurrentBag)
            {
                _unitOfWork.Repository<LocalizationResource>().Create(localization);
            }
            _unitOfWork.Complete();
        }

        private async Task AddResourceToLanguage(Language language, LocalizationResource localizationResource, ConcurrentBag<LocalizationResource> bag)
        {
            try
            {
                var requestData = new
                {
                    text = new[] { localizationResource.Value },
                    target_lang = language.LanguageCode
                };

                var jsonRequestData = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonRequestData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(LocalizationConstants.TranslateUrl, content);
                response.EnsureSuccessStatusCode(); // Ensure successful response

                var responseData = await response.Content.ReadAsStringAsync();
                var translation = JsonConvert.DeserializeObject<TranslatedResponse>(responseData);

                if (translation is null || string.IsNullOrEmpty(translation.TranslatedText))
                {
                    var errorResponse = JsonConvert.DeserializeObject<TranslateErrorResponse>(responseData);
                    throw new Exception(errorResponse?.TranslateError ?? "Translation failed.");
                }

                var newLocalizationResource = new LocalizationResource
                {
                    Key = localizationResource.Key,
                    Namespace = localizationResource.Namespace,
                    BeautifiedNamespace = localizationResource.BeautifiedNamespace,
                    Value = translation.TranslatedText,
                    LanguageId = language.Id,
                    Language = language
                };

                bag.Add(newLocalizationResource);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                Console.WriteLine($"Translation failed: {ex.Message}");
            }
        }

        
        
    }
}
