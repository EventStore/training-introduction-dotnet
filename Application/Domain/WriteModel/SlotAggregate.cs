using System;
using Application.Domain.WriteModel.Events;
using Application.Domain.WriteModel.Exceptions;
using Application.Infrastructure;

namespace Application.Domain.WriteModel;

public class SlotAggregate : AggregateRoot
{
    public SlotAggregate()
    {
        Register<Scheduled>(When);
    }

    public void Schedule(string id, DateTime startTime, TimeSpan duration)
    {
        Raise(null);
    }

    private void When(Scheduled scheduled)
    {
        Id = scheduled.SlotId;
    }
}