namespace Scheduling.Domain.Infrastructure
{
    public class CommandMetadata
    {
        public CorrelationId CorrelationId { get; }

        public CausationId CausationId { get; }

        public string Replayed { get; set; }

        public CommandMetadata(CorrelationId correlationId, CausationId causationId, string replayed)
        {
            CorrelationId = correlationId;
            CausationId = causationId;
            Replayed = replayed;
        }
    }
}
