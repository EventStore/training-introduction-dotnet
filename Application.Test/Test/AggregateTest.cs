using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Domain.WriteModel.Commands;
using Application.Infrastructure;
using Application.Infrastructure.Commands;
using Scheduling.Domain.Infrastructure;
using Scheduling.Domain.Infrastructure.Commands;
using Xunit;

namespace Scheduling.Test
{
    public class AggregateTest<TAggregate> where TAggregate : AggregateRoot
    {
        private Dispatcher _dispatcher;

        private IAggregateStore _repository;

        private AggregateRoot _aggregate;

        private Exception _exception;

        public AggregateTest()
        {
            _aggregate = (AggregateRoot) Activator.CreateInstance(typeof(TAggregate));
            _repository = new FakeAggregateStore(_aggregate);
        }

        public void RegisterHandlers<TCommandHandler>()
            where TCommandHandler : CommandHandler
        {
            var commandHandler = (CommandHandler) Activator.CreateInstance(typeof(TCommandHandler), _repository);
            var commandHandlerMap = new CommandHandlerMap(commandHandler);

            _dispatcher = new Dispatcher(commandHandlerMap);
        }

        protected void Given(params object[] events)
        {
            _exception = null;
            _aggregate.Load(events);
        }

        protected async Task When(object command)
        {
            try
            {
                _aggregate.ClearChanges();
                await _dispatcher.Dispatch(command);
            }
            catch (Exception e)
            {
                _exception = e;
            }
        }

        protected void Then(Action<List<object>> events)
        {
            if (_exception != null)
                throw _exception;

            events(_aggregate.GetChanges().ToList());
        }

        protected void Then<TException>() where TException : Exception
        {
           Assert.Equal(typeof(TException), _exception.GetType());
        }
    }
}
