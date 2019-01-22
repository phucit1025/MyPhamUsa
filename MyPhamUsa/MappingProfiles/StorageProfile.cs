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
                .ReverseMap();

            CreateMap<Storage, IRViewModel>()
                .ReverseMap();
        }
    }
}
