using System;
using Application.Domain.Events;
using Application.Domain.ReadModel;
using Application.EventSourcing;

namespace Application.Application
{
    public class PatientSlotProjector: IEventHandler
    {
        private readonly IPatientSlotsRepository _repository;

        public PatientSlotProjector(IPatientSlotsRepository repository)
        {
            _repository = repository;
        }

        public void Handle(Event @event)
        {
            if (@event is Scheduled s)
            {
                _repository.Add(new ScheduledSlot(s.Id, s.StartTime, s.Duration));
            } 
            if (@event is Booked b)
            {
                _repository.MarkAsBooked(b.SlotId, b.PatientId);
            } 
            if (@event is Cancelled c)
            {
                _repository.MarkAsCancelled(c.Id);
            } 
        }
    }
}