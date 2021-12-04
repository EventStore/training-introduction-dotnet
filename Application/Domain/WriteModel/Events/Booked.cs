using Application.EventSourcing;

namespace Application.Domain.WriteModel.Events;

public record Booked(
    string SlotId,
    string PatientId
): IEvent;
