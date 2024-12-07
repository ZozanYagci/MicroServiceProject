using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Api.IntegrationEvents.Events;
using NotificationService.IntegrationEvents.EventHandlers;


namespace NotificationService
{
    public class Program
    {
        static void Main(string[] args)
        {

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            var sp= services.BuildServiceProvider();
            IEventBus eventBus=sp.GetRequiredService<IEventBus>();

            //Subscribe to events

            SubscribeToEvents(eventBus);

            Console.WriteLine("Application is Running....");

            Console.ReadLine();
        }
        private static void ConfigureServices(ServiceCollection services)
            {
                // Add logging
                services.AddLogging(configure => configure.AddConsole());

                // Add event handlers
                services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();
                services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();

                // Configure EventBus

                services.AddSingleton<IEventBus>(sp =>
                {
                    EventBusConfig config = new()
                    {
                        ConnectionRetryCount = 5,
                        EventNameSuffix = "IntegrationEvent",
                        SubscriberClientAppName = "NotificationService",
                        EventBusType = EventBusType.RabbitMQ
                    };
                    return EventBusFactory.Create(config, sp);
                });
            }

            static void SubscribeToEvents(IEventBus eventBus)
            {
                // Subscribe to integration events with respective handlers
                eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
                eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();

            }
        }
    }
