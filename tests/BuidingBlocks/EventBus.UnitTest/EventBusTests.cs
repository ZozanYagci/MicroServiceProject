using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.UnitTest.Events.EventHandler;
using EventBus.UnitTest.Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EventBus.UnitTest
{
    [TestClass]
    public class EventBusTests
    {
        private ServiceCollection services;
        public EventBusTests()
        {
            services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());

        }
        [TestMethod]
        public void subscribe_event_on_rabbitmq_test()
        {
            services.AddSingleton<IEventBus>(sp =>
            {

                return EventBusFactory.Create(GetRabbitMQConfig(), sp);

            });
            var sp = services.BuildServiceProvider();

            var eventBus = sp.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            //eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }


        //Azure Test
        //    [TestMethod]
        //    public void subscribe_event_on_azure_test()
        //    {
        //        services.AddSingleton<IEventBus>(sp =>
        //        {
        //           
        //            return EventBusFactory.Create(GetAzureConfig(), sp);
        //        });
        //        var sp = services.BuildServiceProvider();

        //        var eventBus = sp.GetRequiredService<IEventBus>();
        //        eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        //        eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();

        //    }


        [TestMethod]
        public void send_message_to_rabbitmq()
        {
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetRabbitMQConfig(), sp);
            });
            var sp=services.BuildServiceProvider();
            var eventBus=sp.GetRequiredService<IEventBus>();
            eventBus.Publish(new OrderCreatedIntegrationEvent(1));
        }


        //Azure Message

        //[TestMethod]
        //public void send_message_to_azure_test()
        //{
        //    services.AddSingleton<IEventBus>(sp =>
        //    {
        //        return EventBusFactory.Create(GetAzureConfig(), sp);
        //    });
        //    var sp = services.BuildServiceProvider();
        //    var eventBus = sp.GetRequiredService<IEventBus>();
        //    eventBus.Publish(new OrderCreatedIntegrationEvent(1));
        //}


       
        private EventBusConfig GetAzureConfig()
        {
            return new EventBusConfig
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "Event.UnitTest",
                DefaultTopicName = "SellingTopicName",
                EventBusType = EventBusType.AzureServiceBus,
                EventNameSuffix = "IntegrationEvent",
                EventBusConnectionString = "",

            };
        }

        private EventBusConfig GetRabbitMQConfig()
        {
            return new EventBusConfig
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "Event.UnitTest",
                DefaultTopicName = "SellingTopicName",
                EventBusType = EventBusType.RabbitMQ,
                EventNameSuffix = "IntegrationEvent",

                // Connection = new ConnectionFactory()
                // {
                //     HostName = "localhost",
                //     Port = 15672,
                //     UserName = "guest",
                //     Password = "guest",
                //}

            };
        }
    }
}