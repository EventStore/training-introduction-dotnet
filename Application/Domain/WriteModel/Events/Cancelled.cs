using System;
using Application.EventSourcing;

namespace Application.Domain.Events
{
    public class Cancelled : Event
    {
        public string Id { get; }
        public string Reason { get; }

        public Cancelled(string id, string reason)
        {
            Id = id;
            Reason = reason;
        }

        protected bool Equals(Cancelled other)
        {
            return Id.Equals(other.Id) && Reason == other.Reason;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Cancelled) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Reason);
        }
    }
}