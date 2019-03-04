using AutoMapper;
using MyPhamUsa.Models.Entities;
using MyPhamUsa.Models.ViewModels;
namespace MyPhamUsa.MappingProfiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderCreateViewModel, Order>();

            CreateMap<Order, OrderViewModel>()
                .ForMember(vm => vm.OrderItems, map => map.MapFrom(dm=>dm.OrderItems));
        }
    }
}
