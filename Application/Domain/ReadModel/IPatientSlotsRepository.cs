using System.Collections.Generic;

namespace Application.Domain.ReadModel;

public interface IPatientSlotsRepository
{
    List<PatientSlot> GetPatientSlots(string patientId);
    void Add(ScheduledSlot scheduledSlot);
    void MarkAsBooked(string id, string patientId);
    void MarkAsCancelled(string scheduledEventId);
    void Clear();
}