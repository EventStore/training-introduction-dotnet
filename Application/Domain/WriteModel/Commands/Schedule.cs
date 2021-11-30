using System;
using Application.EventSourcing;

namespace Application.Domain.WriteModel.Commands;

public record Schedule(
    string Id,
    DateTime StartTime,
    TimeSpan Duration
): ICommand;