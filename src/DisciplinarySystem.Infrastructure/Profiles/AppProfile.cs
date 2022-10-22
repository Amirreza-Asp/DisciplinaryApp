using AutoMapper;
using DisciplinarySystem.Application.Users.ViewModels.Role;
using DisciplinarySystem.Application.Verdicts.ViewModels;
using DisciplinarySystem.Application.Violations.ViewModels.ViolationCategory;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.Domain.Verdicts;
using DisciplinarySystem.Domain.Violations;

namespace DisciplinarySystem.Infrastructure.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<Role, UpdateRole>();


            CreateMap<ViolationCategory, UpdateViolationCategory>();


            CreateMap<Verdict, UpdateVerdict>();
        }
    }
}
