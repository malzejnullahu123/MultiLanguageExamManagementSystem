using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Services.IServices;

namespace MultiLanguageExamManagementSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocalizationsController : ControllerBase
    {
        private readonly ILogger<LocalizationsController> _logger;
        private readonly ICultureService _cultureService;

        public LocalizationsController(ILogger<LocalizationsController> logger, ICultureService cultureService)
        {
            _logger = logger;
            _cultureService = cultureService;
        }

        // Your code here

        [HttpGet("/Localization")]
        public IActionResult GetLocalizationResource(string @namespace, string key)
        // public async Task<IActionResult> GetLocalizationResource()
        {
            // var @namespace = "testspace";
            // var key = "ttt";
            // implement the logic that allows us to call the culture service like this, note that the string we are 
            // sending "ne.1" it is of form namespace.key, in this case "ne" is the namespace and "1" is the key
            // so you should return back the localization resource that is having this namespace and key and the
            // language code based in the request header
            var locator = _cultureService.GetLocator(@namespace, key);
            var message = _cultureService.GetString(locator);
            //var message = _cultureService.GetString(locator).Value; // Implement this too

            
            if (message is null)
            {
                return NotFound($"Localization resource not found for locator: {locator}");
            }

            // Ensure that the resource value is not null or empty
            if (string.IsNullOrEmpty(message.Value))
            {
                return NotFound($"Localization resource value is null or empty for locator: {locator}");
            }

            return Ok(message.Value);
        }
        
        // Your code here
        // Implement endpoints for crud operations (no relation to localization needed here, just normal cruds)
        // Except when adding a new language(or localization resource), you should get all the existing localization resources in english
        // prepare and translate them to the new language using translation service which should use this api for translations:
        // https://www.deepl.com/pro-api?cta=header-pro-api, and then seed the new created localizations for the new added language

        // same applies when adding a new localization resource, for example you implement the code for adding a new
        // language resource, then you call the api to prepare the translated resource to all your existing languages
        // and then you add the same resource for all the languages

        [HttpGet("/Languages")]
        public async Task<IActionResult> GetLanguages()
        {
            var languages = await _cultureService.GetAllLanguages();
            
            return Ok(languages);
        }

        [HttpGet("/Languages/{id}")]
        public async Task<IActionResult> GetLanguageById(int id)
        {
            var language = await _cultureService.GetByIdLanguage(id);
            
            if(language is null)
            {
                return NotFound();
            }
            
            return Ok(language);
        }
        
        
        [HttpPost("/Languages")]
        public async Task<IActionResult> CreateLanguage(LanguageRequestDto languageUpsertDto)
        {
            await _cultureService.CreateLanguage(languageUpsertDto);
            
            return Ok();
        }
        
        [HttpPut("/languages/{id}")]
        public IActionResult UpdateLanguage(int id, LanguageRequestDto languageUpsertDto)
        {
            _cultureService.UpdateLanguage(id, languageUpsertDto);

            return Ok();
        }

        [HttpDelete("/languages/{id}")]
        public IActionResult DeleteLanguage(int id)
        {
            _cultureService.DeleteLanguage(id);

            return Ok();
        }
        
        [HttpGet("/LocalizationResources")]
        public async Task<IActionResult> GetLocalizationResources()
        {
            var localizationResources = await _cultureService.GetAllLocalizationResources();

            return Ok(localizationResources);
        }
        
        [HttpGet("/LocalizationResources/{id:int}")]
        public async Task<IActionResult> GetLocalizationResourceById(int id)
        {
            var localizationResource = await _cultureService.GetLocalizationResourceById(id);

            if(localizationResource is null)
            {
                return NotFound();
            }

            return Ok(localizationResource);
        }
        
        [HttpGet("/LocalizationResources/ByLanguage/{languageId:int}")]
        public async Task<IActionResult> GetLocalizationResourcesByLanguageId(int languageId)
        {
            var localizationResources = await _cultureService.GetLocalizationResourcesByLanguageId(languageId);

            return Ok(localizationResources);
        }
        
        [HttpPost("/LocalizationResources")]
        public async Task<IActionResult> CreateLocalizationResource(LocalizationResourceRequestDto localizationDto)
        {
            var locator = _cultureService.GetLocator(localizationDto.Namespace, localizationDto.Key);
            var message = _cultureService[locator];
            
            await _cultureService.CreateLocalizationResource(localizationDto);

            return Ok();
        }
        
        [HttpPut("/LocalizationResources/{id:int}")]
        public async Task<IActionResult> UpdateLocalizationResource(int id, LocalizationResourceRequestDto localizationResourceUpsertDto)
        {
            await _cultureService.UpdateLocalizationResource(id, localizationResourceUpsertDto);

            return Ok();
        }
        
        [HttpDelete("/LocalizationResources/{namespace}/{key}")]
        public async Task<IActionResult> DeleteLocalizationResource(string @namespace, string key)
        {
            await _cultureService.DeleteLocalizationResource(@namespace, key);

            return Ok();
        }
    }
}
