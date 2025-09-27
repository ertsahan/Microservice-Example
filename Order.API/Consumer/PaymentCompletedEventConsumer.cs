using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Order.API.Models.Entities;
using Shared.Events;

namespace Order.API.Consumer
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly OrderAPIContext _orderAPIContext;

        public PaymentCompletedEventConsumer(OrderAPIContext orderAPIContext)
        {
            _orderAPIContext = orderAPIContext;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
           Order.API.Models.Entities.Order order = await _orderAPIContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.OrderStatus = Models.Enums.OrderStatus.Completed;
            await _orderAPIContext.SaveChangesAsync();
        }
    }
}
