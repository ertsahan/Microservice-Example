using MassTransit;
using MongoDB.Driver;
using Shared;
using Shared.Events;
using Stock.API.Consumers;
using Stock.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        _configurator.ReceiveEndpoint(RabbitMQSettings.Stock_OrderCreatedEventQueue, e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
    });
});

builder.Services.AddSingleton<MongoDbService>();


#region Harici - MongoDB'ye Seed Data Ekleme
using AsyncServiceScope scope = builder.Services.BuildServiceProvider().CreateAsyncScope();
MongoDbService mongoDbService = scope.ServiceProvider.GetService<MongoDbService>();
var collection = mongoDbService.GetCollection<Stock.API.Models.Entities.Stock>();

if (!collection.Find(s => true).Any())
{
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 2000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 1000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 3000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 5000 });
    await collection.InsertOneAsync(new() { ProductId = Guid.NewGuid(), Count = 6000 });
}
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
