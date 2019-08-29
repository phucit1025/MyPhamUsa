using AutoMapper;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
namespace MyPhamUsa.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderCreateViewModel, Order>()
                .ForMember(dm => dm.OrderItems, map => map.Ignore())
                .ReverseMap();

            CreateMap<Order, OrderViewModel>()
                .ForMember(vm => vm.OrderItems, map => map.MapFrom(dm => dm.OrderItems))
                .ForMember(vm => vm.DateCreated, map => map.MapFrom(dm => dm.DateCreated.ToShortDateString()));
        }
    }
}
