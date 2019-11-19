using System;
using AutoMapper;
using MyPhamUsa.Client.Models.Entities;
using MyPhamUsa.Client.Models.ViewModels;

namespace MyPhamUsa.Client.MappingProfiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductTitleViewModel>()
                .ForMember(vm=>vm.Price,map=>map.MapFrom(dm=>dm.Price;
        }
    }
}
