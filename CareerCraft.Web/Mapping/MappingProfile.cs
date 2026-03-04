using AutoMapper;
using CareerCraft.Core.Entities;
using CareerCraft.Web.Models;

namespace CareerCraft.Web.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Skill, SkillViewModel>().ReverseMap();
    }
}
