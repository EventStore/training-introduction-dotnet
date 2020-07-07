namespace Scheduling.Domain.Infrastructure
{
    public class EventMetadata
    {
        public string ClrType { get; set; }

        public string CorrelationId { get; set; }

        public string CausationId { get; set; }

        public string Replayed { get; set; }
    }
}
