using System;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MyPhamUsa.Models.ViewModels;
namespace MyPhamUsa.MappingProfiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<IdentityUser, AccountViewModel>()
                .ForMember(vm => vm.Guid, map => map.MapFrom(dm => dm.Id))
                .ForMember(vm => vm.Username, map => map.MapFrom(dm => dm.UserName));
        }
    }
}
