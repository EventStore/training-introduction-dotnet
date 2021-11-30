using System;

namespace Application.Domain.ReadModel;

public record AvailableSlot(
    string ScheduledId,
    DateTime ScheduledStartTime,
    TimeSpan ScheduledDuration
);