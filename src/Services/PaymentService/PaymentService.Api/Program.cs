using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using PaymentService.Api.IntegrationEvents.EventHandlers;
using PaymentService.Api.IntegrationEvents.Events;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.

ConfigureServices(builder.Services);

var app = builder.Build();

//Run the application
var eventBus = app.Services.GetRequiredService<IEventBus>();

//Subscribe to events
SubscribeToEvents(eventBus);


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


void ConfigureServices(IServiceCollection services)
{
    //Add logging 
    services.AddLogging(configure => configure.AddConsole());

    // Add event handlers
    services.AddTransient<OrderStartedIntegrationEventHandler>();

    // Configure EventBus
    services.AddSingleton<IEventBus>(sp =>
    {
   
        EventBusConfig config = new()
        {
            ConnectionRetryCount = 5,
            EventNameSuffix = "IntegrationEvent",
            SubscriberClientAppName = "PaymentService",
            EventBusType = EventBusType.RabbitMQ
        };
        return EventBusFactory.Create(config, sp);
    });

}

void SubscribeToEvents(IEventBus eventBus)
{
    eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();

}


