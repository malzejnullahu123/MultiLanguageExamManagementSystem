using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services.IServices;
using System.Globalization;
using AutoMapper;

namespace MultiLanguageExamManagementSystem.Services
{
    public class CultureService : ICultureService
    {
        
    private readonly IUnitOfWork _unitOfWork;
    private readonly TranslationService  _translationService;
    private readonly IMapper _mapper;

    public CultureService(IUnitOfWork unitOfWork, IMapper mapper, TranslationService translationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _translationService = translationService;
    }
    
    #region String Localization
    
    public LocalizationResource this[string locator] => GetString(locator);

    public LocalizationResource GetString(string locator)
    {
        var locatorParts = locator.Split('.');
        if (locatorParts.Length != 2) return null;

        var @namespace = locatorParts[0];
        var key = locatorParts[1];
        var countryCode = GetCountryCode();
        var languageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
        
        var resource = _unitOfWork
            .Repository<LocalizationResource>()
            .GetByCondition(x =>
                x.Key == key &&
                x.Namespace == @namespace &&
                x.Language.LanguageCode == languageCode 
                )
            .FirstOrDefault();
        
        return resource;
    }

    public string GetLocator(string @namespace, string key)
    {
        return $"{@namespace}.{key}";
    }

    private string GetCountryCode()
    {
        var regionInfo = new RegionInfo(CultureInfo.CurrentCulture.Name);
        
        return regionInfo.TwoLetterISORegionName;
    }
    
    #endregion

    #region Languages

    public async Task<List<LanguagesResponseDto>> GetAllLanguages()
    {
        var languages = await _unitOfWork.Repository<Language>()
            .GetAll()
            .Select(x => _mapper.Map<LanguagesResponseDto>(x))
            .ToListAsync();

        return languages;
    }

    public async Task<LanguagesResponseDto> GetByIdLanguage(int id)
    {
        var language = await _unitOfWork.Repository<Language>()
            .GetById(x => x.Id == id)
            .Select(x => _mapper.Map<LanguagesResponseDto>(x))
            .FirstOrDefaultAsync();
        
        return language;
    }

    public async Task CreateLanguage(LanguageRequestDto languageRequestDto)
    {
        var languageExists = _unitOfWork.Repository<Language>()
            .GetByCondition(x => x.LanguageCode == languageRequestDto.LanguageCode && x.CountryId  == languageRequestDto.CountryId)
            .Any();
        
        if (languageExists)
        {
            return;
        }

        var language = _mapper.Map<Language>(languageRequestDto);
        _unitOfWork.Repository<Language>().Create(language); 
        _unitOfWork.Complete();

        var defaultLanguage = await _unitOfWork.Repository<Language>().GetByCondition(x => x.IsDefault).FirstOrDefaultAsync();
        var queryable = _unitOfWork.Repository<LocalizationResource>().GetAll();

        queryable = defaultLanguage is null
            ? queryable.GroupBy(x => new {x.Namespace, x.Key})
                .Select(x => x.First())
            : queryable.Where(x => x.LanguageId == defaultLanguage.Id);
        
        var resourcesToTranslate = await queryable.ToListAsync();
        await _translationService.TranslateForLanguage(language, resourcesToTranslate);
    }

    public async Task UpdateLanguage(int id, LanguageRequestDto languageRequestDto)
    {
        var language = await _unitOfWork.Repository<Language>().GetById(x => x.Id == id).FirstOrDefaultAsync();

        if (language is null)
        {
            return;
        }
        
        _mapper.Map(languageRequestDto, language);
        _unitOfWork.Repository<Language>().Update(language);
        _unitOfWork.Complete();
    }

    public async Task DeleteLanguage(int id)
    {
        var language = await _unitOfWork.Repository<Language>().GetById(x => x.Id == id).FirstOrDefaultAsync();
        if (language != null)
        {
            _unitOfWork.Repository<Language>().Delete(language);
        }
    }

    #endregion

    #region Localization Resources

    
    public async Task<List<LocalizationResourceResponseDto>> GetAllLocalizationResources()
    {
        var localizationResources = await _unitOfWork.Repository<LocalizationResource>()
            .GetAll()
            .Select(x => _mapper.Map<LocalizationResourceResponseDto>(x))
            .ToListAsync();

        return localizationResources;
    }
    
    
    public async Task<LocalizationResourceResponseDto> GetLocalizationResourceById(int id)
    {
        var localizationResource = await _unitOfWork.Repository<LocalizationResource>()
            .GetById(x => x.Id == id)
            .Select(x => _mapper.Map<LocalizationResourceResponseDto>(x))
            .FirstOrDefaultAsync();

        return localizationResource;
    }
    
    
    public async Task<List<LocalizationResourceResponseDto>> GetLocalizationResourcesByLanguageId(int languageId)
    {
        var localizationResources = await _unitOfWork.Repository<LocalizationResource>()
            .GetByCondition(x => x.LanguageId == languageId)
            .Select(x => _mapper.Map<LocalizationResourceResponseDto>(x))
            .ToListAsync();

        return localizationResources;
    }
    
    public async Task CreateLocalizationResource(LocalizationResourceRequestDto resourceDto)
    {
        var resource = _mapper.Map<LocalizationResource>(resourceDto);
        
        var missingLanguages = await _unitOfWork.Repository<Language>().GetByCondition(x => x.Id != resourceDto.LanguageId).ToListAsync();
        
        _unitOfWork.Repository<LocalizationResource>().Create(resource);
        await _translationService.TranslateForResource(resource, missingLanguages);
        _unitOfWork.Complete();
    }
    
    public async Task UpdateLocalizationResource(int id, LocalizationResourceRequestDto resourceDto)
    {
        var resource = await _unitOfWork.Repository<LocalizationResource>().GetById(x => x.Id == id).FirstOrDefaultAsync();

        if (resource is null)
        {
            return;
        }

        _mapper.Map(resourceDto, resource);
        _unitOfWork.Repository<LocalizationResource>().Update(resource);
        _unitOfWork.Complete();
    }
    
    public async Task DeleteLocalizationResource(string @namespace, string key)
    {
        var resource = await _unitOfWork.Repository<LocalizationResource>().GetByCondition(x => x.Namespace == @namespace && x.Key == key).ToListAsync();
        _unitOfWork.Repository<LocalizationResource>().DeleteRange(resource);
        _unitOfWork.Complete();
    }
    
    #endregion
    }
}
