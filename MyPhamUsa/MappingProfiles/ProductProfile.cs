using System;
using AutoMapper;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
namespace MyPhamUsa.MappingProfiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(vm=>vm.AvailableQuantity,map=>map.MapFrom(dm=>dm.QuantityIndex))
                .ReverseMap()
                .ForMember(dm=>dm.Id,map=>map.Ignore())
                .ForMember(dm=>dm.DateUpdated,map=>map.MapFrom(vm=>DateTime.Now));

            CreateMap<Product, ProductCreateViewModel>()
                .ReverseMap();
        }
    }
}
