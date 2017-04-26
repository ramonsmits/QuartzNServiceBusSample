using System;
using System.Runtime.InteropServices;
using NServiceBus;

namespace QuartzNServiceBusSample.Host
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
        public void Customize(EndpointConfiguration configuration)
        {
            Console.Title = "Host";

            log4net.Config.XmlConfigurator.Configure();
            log4net.LogManager.GetLogger("Test").Info("Test");

            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseTransport<MsmqTransport>();
            configuration.SendFailedMessagesTo("error");
        }
    }
}