using AutoMapper;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace LifeEcommerce.Helpers
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {
            CreateMap<Language, LanguagesResponseDto>().ReverseMap();
            CreateMap<LanguagesResponseDto, Language>().ReverseMap();

            CreateMap<Language, LanguageRequestDto>().ReverseMap();
            CreateMap<LanguageRequestDto, Language>().ReverseMap();

            CreateMap<LocalizationResource, LocalizationResourceResponseDto>().ReverseMap();
            CreateMap<LocalizationResourceResponseDto, LocalizationResource>().ReverseMap();

            CreateMap<LocalizationResource, LocalizationResourceRequestDto>().ReverseMap();
            CreateMap<LocalizationResourceRequestDto, LocalizationResource>().ReverseMap();
            
            
            CreateMap<User, UserRequestDto>().ReverseMap();
            CreateMap<UserRequestDto, User>().ReverseMap();
            
            CreateMap<LocalizationResource, LocalizationResource>();
            
        }
    }
}
