using Scheduling.Domain.Infrastructure;
using Scheduling.Domain.Infrastructure.Commands;

namespace Application.Domain.WriteModel.Commands
{
    public class Handlers : CommandHandler
    {
        public Handlers(IAggregateStore aggregateStore)
        {
            Register<Schedule>(async s =>
            {

            });
        }
    }
}
