using System;
using AutoMapper;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
using System.Linq;
namespace MyPhamUsa.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(vm => vm.AvailableQuantity, map => map.MapFrom(dm => dm.QuantityIndex))
                .ForMember(vm=>vm.ImagePaths,map=>map.MapFrom(dm=>dm.Images.Where(i=>!i.IsDeleted).Select(i=>i.Path).ToList()))
                .ReverseMap()
                .ForMember(dm => dm.Id, map => map.Ignore())
                .ForMember(dm => dm.DateUpdated, map => map.MapFrom(vm => DateTime.Now));

            CreateMap<Product, ProductOfStaffViewModel>()
                .ForMember(vm => vm.AvailableQuantity, map => map.MapFrom(dm => dm.QuantityIndex))
                .ForMember(vm => vm.ImagePaths, map => map.MapFrom(dm => dm.Images.Where(i => !i.IsDeleted).Select(i => i.Path).ToList()))
                .ReverseMap()
                .ForMember(dm => dm.Id, map => map.Ignore())
                .ForMember(dm => dm.DateUpdated, map => map.MapFrom(vm => DateTime.Now));

            CreateMap<Product, ProductCreateViewModel>()
                .ForMember(vm => vm.ReceiveQuantity, map => map.MapFrom(dm => dm.QuantityIndex))
                .ForMember(vm => vm.Price, map => map.MapFrom(dm => Convert.ToInt32(dm.Price)))
                .ForMember(vm => vm.SellPrice, map => map.MapFrom(dm => Convert.ToInt32(dm.SellPrice)))
                .ReverseMap()
                .ForMember(dm => dm.Price, map => map.MapFrom(vm => vm.Price.ToString()))
                .ForMember(dm => dm.SellPrice, map => map.MapFrom(vm => vm.SellPrice.ToString()))
                .ForMember(dm => dm.QuantityIndex, map => map.MapFrom(vm => vm.ReceiveQuantity));
        }
    }
}
