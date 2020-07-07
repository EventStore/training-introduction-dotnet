using System;
using System.Collections.Generic;
using Application.Domain.Events;
using Application.EventSourcing;

namespace Application.Domain
{
    public class SlotState : AggregateState
    {
        public string SlotId { get; set; }
        private string _PatientId { get; set; }
        private DateTime? _StartTime { get; set; }

        public bool IsScheduled()
        {
            return !String.IsNullOrEmpty(SlotId);
        }

        public bool IsBooked()
        {
            return !String.IsNullOrEmpty(_PatientId);
        }

        public override void Apply(List<Event> events)
        {
            foreach (var @event in events)
            {
                if (@event is Scheduled s)
                {
                    SlotId = s.Id;
                    _StartTime = s.StartTime;
                }
                if (@event is Booked b)
                {
                    _PatientId = b.PatientId;
                }
                if (@event is Cancelled)
                {
                    _PatientId = null;
                }
            }
        }

        public bool IsStarted(Func<DateTime> now)
        {
            return now().CompareTo(_StartTime) > 0;
        }
    }
}