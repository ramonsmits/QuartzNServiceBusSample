using System;
using Common.Logging;
using NServiceBus;
using NServiceBus.Installation;
using Quartz;
using QuartzNServiceBusSample.Messages;

namespace QuartzNServiceBusSample
{
    public class DoSomethingSchedule : ScheduleSetup<DoSomethingJob>
    {
        private readonly ILog Log = LogManager.GetLogger<DoSomethingSchedule>();

        public DoSomethingSchedule(IScheduler scheduler) : base(scheduler)
        {
            Log.Debug("Constructor");
        }

        protected override TriggerBuilder CreateTrigger()
        {
            Log.Debug("Create trigger");
            return TriggerBuilder.Create().WithCalendarIntervalSchedule(b => b.WithIntervalInSeconds(5));
        }
    }

    public class DoSomethingJob : IJob
    {
        readonly ILog Log = LogManager.GetLogger<DoSomethingJob>();
        readonly IMessageSession Session;


        public DoSomethingJob(QuartzService instance)
        {
            Log.Debug("Constructor");
            Session = instance.Session;
        }

        public void Execute(IJobExecutionContext context)
        {
            Log.Info("Executing");
            Session.Send(new DoSomething()).GetAwaiter().GetResult();
            Log.Info("Executed");
        }
    }
}
