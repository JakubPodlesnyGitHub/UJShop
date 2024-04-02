﻿using AutoMapper;
using Shop.API.CQRS.Commands.OrderItem;
using Shop.Domain.Domain;
using Shop.Shared.Dtos.Response;

namespace Shop.API.Mappings
{
    public class OrderItemProfile : Profile
    {
        public OrderItemProfile()
        {
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<OrderItemDTO, OrderItem>();
            CreateMap<AddedOrderItemCommand, OrderItem>();
            CreateMap<EditedOrderItemCommand, OrderItem>();
        }
    }
}