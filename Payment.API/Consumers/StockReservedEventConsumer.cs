﻿using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            // Ödeme İşlemleri...
            if(true)
            {
                // Ödemenin Başarıyla Tamamlandığını...
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };
                _publishEndpoint.Publish(paymentCompletedEvent);
                Console.WriteLine("Ödeme Başarılı");
            }
            else
            {
                // Ödemede Sıkıntı Olduğunu...
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Bakiye Yetersiz!"
                };
                _publishEndpoint.Publish(paymentFailedEvent);
                Console.WriteLine("Ödeme Başarısız...");
            }
                return Task.CompletedTask; 
        }
    }
}
