using AutoMapper;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
namespace MyPhamUsa.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryViewModel>()
                .ReverseMap();

            CreateMap<Category, CategoryCreateViewModel>()
                .ReverseMap()
                .ForMember(dm => dm.Id, map => map.Ignore());

            CreateMap<ProductCategory, CategoryViewModel>()
                .ForMember(vm => vm.Id, map => map.MapFrom(dm => dm.CategoryId))
                .ForMember(vm => vm.Name, map => map.MapFrom(dm => dm.Category.Name));
        }
    }
}
