using System;

namespace Scheduling.Domain.Infrastructure
{
    public class CorrelationId : Value<CorrelationId>
    {
        public Guid Value { get; }

        public CorrelationId(string value) =>
            Value = Guid.Parse(value);
    }
}
