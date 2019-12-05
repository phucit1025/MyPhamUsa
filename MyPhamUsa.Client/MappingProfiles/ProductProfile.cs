using AutoMapper;
using MyPhamUsa.Client.Models.Entities;
using MyPhamUsa.Client.Models.ViewModels;
using System.Linq;

namespace MyPhamUsa.Client.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductTitleViewModel>()
                .ForMember(vm => vm.Price, map => map.MapFrom(dm => dm.SellPrice))
                .ForMember(vm => vm.Image, map => map.MapFrom(dm => dm.Images.FirstOrDefault(i => !i.IsDeleted).Path));

            CreateMap<Product, ProductDetailViewModel>()
                .ForMember(vm => vm.Price, map => map.MapFrom(dm => dm.SellPrice))
                .ForMember(vm => vm.IsInStock, map => map.MapFrom(dm => dm.QuantityIndex == 0 ? false : true))
                .ForMember(vm => vm.Images, map => map.MapFrom(dm => dm.Images.Where(i => !i.IsDeleted)));

            CreateMap<Image, ImageViewModel>()
                ;
        }
    }
}
