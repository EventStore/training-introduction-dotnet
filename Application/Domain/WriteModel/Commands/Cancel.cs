using System;
using Application.EventSourcing;

namespace Application.Domain.WriteModel.Commands
{
    public class Cancel : Command
    {
        public string Id { get; }

        public string Reason { get; }

        public DateTime CancellationTime { get; }

        public Cancel(string id, string reason, DateTime cancellationTime)
        {
            Id = id;
            Reason = reason;
            CancellationTime = cancellationTime;
        }
    }
}
