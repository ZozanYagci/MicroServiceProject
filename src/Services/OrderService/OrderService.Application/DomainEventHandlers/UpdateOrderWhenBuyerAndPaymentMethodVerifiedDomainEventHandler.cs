﻿using MediatR;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DomainEventHandlers
{
    public class UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler : INotificationHandler<BuyerAndPaymentMethodVerifiedDomainEvent>
    {
        private readonly IOrderRepository orderRepository;

        public UpdateOrderWhenBuyerAndPaymentMethodVerifiedDomainEventHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task Handle(BuyerAndPaymentMethodVerifiedDomainEvent notification, CancellationToken cancellationToken)
        {
            var orderToUpdate = await orderRepository.GetByIdAsync(notification.OrderId);
            orderToUpdate.SetBuyerId(notification.Buyer.Id);
            orderToUpdate.SetPaymentMethodId(notification.Payment.Id);

        }

    }
}
