using System.Linq;
using Application.EventSourcing;
using Xunit;

namespace Application.Test.Test
{
    public abstract class EventHandlerTest
    {
        protected ReplayableIdGenerator _idGenerator = new ReplayableIdGenerator();
        protected abstract IEventHandler EventHandler();

        private IEventHandler _handler;

        protected void Given(params Event[] events)
        {
            _handler = EventHandler();
            events.ToList().ForEach(_handler.Handle);
        }

        protected void Then(object expected, object actual)
        {
            Assert.Equal(expected, actual);
        }
    }
}