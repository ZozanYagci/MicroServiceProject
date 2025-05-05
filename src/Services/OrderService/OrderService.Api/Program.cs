using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.EntityFrameworkCore;
using OrderService.Api.Extensions;
using OrderService.Api.Extensions.Registration.EventHandlerRegistration;
using OrderService.Api.Extensions.Registration.ServiceDiscovery;
using OrderService.Api.IntegrationEvents.EventHandlers;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Context;


var builder = WebApplication.CreateBuilder(args);

//Logging
builder.Services.AddLogging(configure => configure.AddConsole());


//Application, Infrastructure, and Event Handler Registrations
builder.Services
    .AddApplicationRegistration(typeof(Program))
    .AddPersistenceRegistration(builder.Configuration)
    .ConfigurationEventHandlers()
    .AddServiceDiscoveryRegistration(builder.Configuration);

//EventBus Configuration
builder.Services.AddSingleton(sp =>
{
    var config = new EventBusConfig
    {
        ConnectionRetryCount = 5,
        EventNameSuffix = "IntegrationEvent",
        SubscriberClientAppName = "OrderService",
        EventBusType = EventBusType.RabbitMQ,

    };
    return EventBusFactory.Create(config, sp);
});


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDbConnectionString")));


var app = builder.Build();

//----Migration & Seeding------
app.MigrateDbContext<OrderDbContext>((context, services) =>
{
    var logger = services.GetRequiredService<ILogger<OrderDbContext>>();
    var seeder = new OrderDbContextSeed();
    seeder.SeedAsync(context, logger).Wait();
});

//----Event Subscription--------
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();

