using Application.EventSourcing;

namespace Application.Domain.WriteModel.Commands;

public record Cancel(
): ICommand;