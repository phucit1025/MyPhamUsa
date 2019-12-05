using AutoMapper;
using MyPhamUsa.Client.Models.Entities;
using MyPhamUsa.Client.Models.ViewModels;

namespace MyPhamUsa.Client.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryViewModel>();
        }
    }
}
