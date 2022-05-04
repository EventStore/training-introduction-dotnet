using System;
using Application.Domain.WriteModel.Events;
using Application.Domain.WriteModel.Exceptions;
using Application.Infrastructure;

namespace Application.Domain.WriteModel;

public class SlotAggregate : AggregateRoot
{
    private bool _isBooked = false; 
    private bool _isScheduled = false;
    private DateTime? _startTime = null;
    
    public SlotAggregate()
    {
        Register<Scheduled>(When);
        Register<Cancelled>(When);
        Register<Booked>(When);
    }

    public void Schedule(string id, DateTime startTime, TimeSpan duration)
    {
        // todo raise correct event here
        Raise(null);
    }
    
    public void Cancel(string reason, DateTime cancellationTime)
    {
    }

    public void Book(string patientId)
    {
    }


    private void When(Scheduled scheduled)
    {
        Id = scheduled.SlotId;
    }
    
    private void When(Cancelled cancelled)
    {
    }
    private void When(Booked booked)
    {
    }
}