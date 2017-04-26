using System;
using System.Threading.Tasks;
using Common.Logging;
using NServiceBus;
using Quartz;

namespace QuartzNServiceBusSample
{
    public class QuartzService : IWantToRunWhenEndpointStartsAndStops
    {
        private readonly ILog Log = LogManager.GetLogger<QuartzService>();
        private readonly IScheduler _scheduler;
        public IMessageSession Session { get; private set; }


        public QuartzService(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task Start(IMessageSession session)
        {
            Session = session;
            Log.Debug("Starting");
            _scheduler.Start();
            Log.Debug("Started");
            return Task.CompletedTask;
        }

        public Task Stop(IMessageSession session)
        {
            Log.Debug("Stopping");
            _scheduler.Shutdown(true);
            Log.Debug("Stopped");
            return Task.CompletedTask;
        }
    }
}