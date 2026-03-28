using System;
using FoodNet.Contracts;
using Ordering.API.Dtos;
using Ordering.API.Entities;
using Ordering.API.EventBusConsumer;

namespace Ordering.API.Mappers;

public static class OrderMapper
{
    public static Order ToOrder(this BasketCheckoutEvent basketCheckoutEvent)
    {
        return new Order
        {
            UserId = basketCheckoutEvent.UserId,
            TotalPrice = basketCheckoutEvent.TotalPrice,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Submitted,

            Items = basketCheckoutEvent.Items.Select(i => new OrderItemDetails
            {
                Quantity = i.Quantity,
                ItemName = i.ItemName,
                Price = i.Price,
                ItemId = i.ItemId
            }).ToList()
        };
    }

    public static OrderDto ToOrderDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalPrice = order.TotalPrice,
            Status = order.Status,
            Items = order.Items
        };
    }

}
