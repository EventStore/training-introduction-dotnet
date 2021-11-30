using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scheduling.Domain.Infrastructure.Commands;

public class CommandHandler
{
    internal Dictionary<string, Func<object, Task>> Handlers { get; } =
        new Dictionary<string, Func<object, Task>>();

    protected void Register<T>(Func<T, Task> handler) =>
        Handlers.Add(typeof(T).Name, (c) => handler((T) c));
}