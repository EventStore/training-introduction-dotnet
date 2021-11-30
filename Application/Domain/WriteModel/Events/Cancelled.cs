using Application.EventSourcing;

namespace Application.Domain.WriteModel.Events;

public record Cancelled(
    string SlotId,
    string Reason
): IEvent;
