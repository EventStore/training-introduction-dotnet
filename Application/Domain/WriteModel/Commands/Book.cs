using Application.EventSourcing;

namespace Application.Domain.WriteModel.Commands;

public record Book(
    string Id,
    string PatientId
): ICommand;