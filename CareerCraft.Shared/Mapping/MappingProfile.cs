using AutoMapper;
using CareerCraft.Core.Entities;
using CareerCraft.Shared.ViewModels;

namespace CareerCraft.Shared.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Skill, SkillViewModel>().ReverseMap();
        CreateMap<User, UserViewModel>().ReverseMap();
        CreateMap<UserInfo, UserInfoViewModel>().ReverseMap();

        // Mapping pour la synchronisation des Vacancies
        CreateMap<ExternalVacancy, Vacancy>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.SourceName, opt => opt.Ignore())
            .ForMember(dest => dest.ExternalId, opt => opt.Ignore())
            .ForMember(dest => dest.LastSyncDateTime, opt => opt.Ignore());
    }
}
