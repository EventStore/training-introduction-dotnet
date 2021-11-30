using System;

namespace Application.Domain.ReadModel;

public record ScheduledSlot(
    string Id,
    DateTime StartTime,
    TimeSpan Duration
);