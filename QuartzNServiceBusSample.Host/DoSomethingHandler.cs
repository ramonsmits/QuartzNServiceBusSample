using System.Threading.Tasks;
using NServiceBus;
using QuartzNServiceBusSample.Messages;
using log4net;

namespace QuartzNServiceBusSample.Host
{
    public class DoSomethingHandler : IHandleMessages<DoSomething>
    {
        public async Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            LogManager.GetLogger(GetType()).Info("Doing something");
        }
    }
}