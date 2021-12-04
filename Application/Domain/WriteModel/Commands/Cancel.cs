using System;
using Application.EventSourcing;

namespace Application.Domain.WriteModel.Commands;

public record Cancel(
    string Id,
    string Reason,
    DateTime CancellationTime
): ICommand;