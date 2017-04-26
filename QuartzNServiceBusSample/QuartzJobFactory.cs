using System;
using Common.Logging;
using NServiceBus.ObjectBuilder;
using NServiceBus.ObjectBuilder.Common;
using Quartz;
using Quartz.Spi;

namespace QuartzNServiceBusSample
{
    public class QuartzJobFactory : IJobFactory
    {
        private readonly ILog Log = LogManager.GetLogger<QuartzJobFactory>();
        private readonly IBuilder _container;

        public QuartzJobFactory(IBuilder container)
        {
            Log.Debug("Constructor");
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            Log.Debug("NewJob");
            return _container.Build(bundle.JobDetail.JobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            Log.Debug("ReturnJob");
            _container.Release(job);
        }
    }
}