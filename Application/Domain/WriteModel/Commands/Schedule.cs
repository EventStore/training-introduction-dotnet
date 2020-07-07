using System;
using Application.EventSourcing;

namespace Application.Domain.WriteModel.Commands
{
    public class Schedule : Command
    {
        public string Id { get; }

        public DateTime StartTime { get; }

        public TimeSpan Duration { get; }

        public Schedule(string id, DateTime startTime, TimeSpan duration)
        {
            Id = id;
            StartTime = startTime;
            Duration = duration;
        }
    }
}
