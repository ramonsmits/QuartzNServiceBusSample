using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Common.Logging;
using NServiceBus;
using NServiceBus.Installation;
using Quartz;

namespace QuartzNServiceBusSample
{
    public abstract class ScheduleSetup<TJob> 
        : IWantToRunWhenEndpointStartsAndStops
        where TJob : IJob
    {
        private readonly ILog Log = LogManager.GetLogger<QuartzService>();
        private readonly IScheduler _scheduler;

        protected ScheduleSetup(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        protected abstract TriggerBuilder CreateTrigger();

        public Task Start(IMessageSession session)
        {
            Log.Debug("Starting");
            var typeOfJob = typeof(TJob);
            var jobName = typeOfJob.Name;
            var jobKey = new JobKey(jobName);

            var jobDetail = JobBuilder.Create<TJob>().WithIdentity(jobKey).Build();
            var trigger = CreateTrigger().ForJob(jobDetail).Build();

            if (_scheduler.GetJobDetail(jobKey) == null)
            {
                _scheduler.ScheduleJob(jobDetail, trigger);
            }
            else
            {
                var triggerName = typeof(TJob).Name + "-CronTrigger";

                _scheduler.RescheduleJob(new TriggerKey(triggerName), trigger);
            }
            Log.Debug("Started");
            return Task.CompletedTask;
        }

        public Task Stop(IMessageSession session)
        {
            Log.Debug("Stop");
            return Task.CompletedTask;
        }
    }
}