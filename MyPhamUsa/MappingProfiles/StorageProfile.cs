using System;
using AutoMapper;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;

namespace MyPhamUsa.MappingProfiles
{
    public class StorageProfile : Profile
    {
        public StorageProfile()
        {
            CreateMap<Storage, StorageViewModel>()
                .ForMember(vm => vm.Date, map => map.MapFrom(dm => dm.DateCreated.ToShortDateString()))
                .ForMember(vm => vm.Time, map => map.MapFrom(dm => dm.DateCreated.ToShortTimeString()))
                .ReverseMap();

            CreateMap<Storage, IRViewModel>()
                .ReverseMap();
        }
    }
}
