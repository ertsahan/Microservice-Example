using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumer
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly OrderAPIContext _orderAPIContext;

        public StockNotReservedEventConsumer(OrderAPIContext orderAPIContext)
        {
            _orderAPIContext = orderAPIContext;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            Order.API.Models.Entities.Order order = await _orderAPIContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
            order.OrderStatus = Models.Enums.OrderStatus.Failed;
            await _orderAPIContext.SaveChangesAsync();
        }
    }
}
