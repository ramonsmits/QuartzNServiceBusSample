using System;
using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.ObjectBuilder;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using QuartzNServiceBusSample.Messages;

namespace QuartzNServiceBusSample
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Client
    {
        public void Customize(EndpointConfiguration configuration)
        {
            Console.Title = "Scheduler";

            log4net.Config.XmlConfigurator.Configure();
            log4net.LogManager.GetLogger("Test").Info("Test");

            NServiceBus.Logging.LogManager.Use<Log4NetFactory>();

            configuration.UsePersistence<InMemoryPersistence>();
            var transport = configuration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(DoSomething).Assembly, "QuartzNServiceBusSample.Host");
            configuration.SendFailedMessagesTo("error");

            configuration.SendOnly();

            configuration.RegisterComponents(components =>
            {
                components.ConfigureComponent<IJobFactory>(builder => new QuartzJobFactory(builder), DependencyLifecycle.InstancePerUnitOfWork);

                components.ConfigureComponent<IScheduler>(builder =>
                {
                    var factoryx = new StdSchedulerFactory();
                    factoryx.Initialize();

                    var scheduler = factoryx.GetScheduler();
                    scheduler.JobFactory = builder.Build<IJobFactory>();
                    return scheduler;

                }, DependencyLifecycle.SingleInstance);

                components.ConfigureComponent<DoSomethingJob>(DependencyLifecycle.InstancePerUnitOfWork);

                components.ConfigureComponent<QuartzService>(DependencyLifecycle.SingleInstance);
                components.ConfigureComponent<IMessageSession>(b =>
                    {

                        var s = b.Build<QuartzService>().Session;
                        if (s == null) throw new InvalidOperationException();
                        return s;
                    }
                    , DependencyLifecycle.SingleInstance);
            });
        }
    }
}