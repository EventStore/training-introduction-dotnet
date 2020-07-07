using System;

namespace Scheduling.Domain.Infrastructure
{
    public class CausationId
    {
        public Guid Value { get; }

        public CausationId(string value) =>
            Value = Guid.Parse(value);
    }
}
