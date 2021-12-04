using System;
using Application.EventSourcing;

namespace Application.Domain.WriteModel.Events;

public record Scheduled(
    string SlotId,
    DateTime StartTime,
    TimeSpan Duration
): IEvent;