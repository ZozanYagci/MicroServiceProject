﻿using MediatR;
using OrderService.Application.Features.Queries.ViewModels;


namespace OrderService.Application.Features.Queries.GetOrderDetailById
{
    public class GetOrderDetailsQuery : IRequest<OrderDetailViewModel>
    {
        public Guid OrderId { get; set; }

        public GetOrderDetailsQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }

}
