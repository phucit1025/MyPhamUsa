using AutoMapper;
using MyPhamUsa.Client.Models.Entities;
using MyPhamUsa.Client.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Client.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderItemCreateViewModel, OrderItem>()
                .ForMember(dm => dm.Id, map => map.Ignore())
                .ForMember(dm => dm.ProductId, map => map.MapFrom(vm => vm.Id));
        }
    }
}
