using FoodNet.Contracts;
using MassTransit;
using Ordering.API.Interfaces;
using Ordering.API.Mappers;


namespace Ordering.API.EventBusConsumer;

public class BasketCheckoutConsumer(IOrderRepository orderRepository) : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        var order = context.Message.ToOrder();
        await orderRepository.AddOrderAsync(order);
    }
}
