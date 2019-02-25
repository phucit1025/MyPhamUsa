using System;
using AutoMapper;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
using System.Linq;

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

            CreateMap<Storage, OrderItemViewModel>()
                .ForMember(vm => vm.ProductId, map => map.MapFrom(dm => dm.ProductId))
                .ForMember(vm => vm.Quantity, map => map.MapFrom(dm => dm.Quantity))
                .ForMember(vm => vm.Name, map => map.MapFrom(dm => dm.Product.Name))
                .ForMember(vm => vm.Price, map => map.MapFrom(dm => dm.Product.SellPrice))
                .ForMember(vm => vm.ImagePaths,map=>map.MapFrom(dm=>dm.Product.Images.Where(i=>!i.IsDeleted).Select(i=>i.Path).ToList()));
        }
    }
}
