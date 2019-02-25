using System;
using AutoMapper;
using MyPhamUsa.Models.ViewModels;
using MyPhamUsa.Models.Entities;
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
