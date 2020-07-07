using System;
using System.Collections.Generic;
using System.Linq;
using Application.Domain.ReadModel;

namespace Application.Infrastructure.InMemory
{
    public class InMemoryPatientSlotsRepository : IPatientSlotsRepository
    {
        private List<ScheduledSlot> _scheduledSlots = new List<ScheduledSlot>();

        private readonly Dictionary<String, List<PatientSlot>> _patientSlots =
            new Dictionary<string, List<PatientSlot>>();

        public List<PatientSlot> getPatientSlots(string patientId)
        {
            if (_patientSlots.ContainsKey(patientId))
            {
                return _patientSlots.First(pair => pair.Key == patientId).Value;
            }
            else
            {
                return new List<PatientSlot>();
            }
        }

        public void Add(ScheduledSlot scheduledSlot)
        {
            _scheduledSlots.Add(scheduledSlot);
        }

        public void MarkAsBooked(string id, string patientId)
        {
            var scheduled = _scheduledSlots.First(s => s.Id == id);
            var patientSlot = new PatientSlot(scheduled.Id, scheduled.StartTime, scheduled.Duration);
            if (_patientSlots.ContainsKey(patientId))
            {
                var patientSlots = _patientSlots.First(pair => pair.Key == patientId).Value;
                patientSlots.Add(patientSlot);
            }
            else
            {
                _patientSlots.Add(patientId, new List<PatientSlot> {patientSlot});
            }

            _scheduledSlots.Remove(scheduled);
        }

        public void MarkAsCancelled(string scheduledEventId)
        {
            var allSlots = _patientSlots.Values.SelectMany(slots => slots.ToArray());
            var slot = allSlots.First(slot => slot.ScheduledId.Equals(scheduledEventId));
            slot.MarkAsCancelled();
            
            _scheduledSlots.Add(new ScheduledSlot(slot.ScheduledId, slot.StartTime, slot.Duration));
        }
    }
}