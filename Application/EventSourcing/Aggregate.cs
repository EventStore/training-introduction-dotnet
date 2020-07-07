using System;
using System.Collections.Generic;

namespace Application.EventSourcing
{
    public abstract class Aggregate<T, U> where U : AggregateState
    {
        private readonly string _id;
        protected readonly Func<Guid> _idGenerator;
        private List<Event> _changes = new List<Event>();
        protected U _state;
        private long _version = -1;

        public Aggregate(string id, Func<Guid> idGenerator, U state)
        {
            _id = id;
            _idGenerator = idGenerator;
            _state = state;
        }

        public abstract List<Event> CalculateChanges(Command command);

        public void Handle(Command command)
        {
            _changes.AddRange(CalculateChanges(command));
        }

        public void Reconstitute(List<Event> events)
        {
            _state.Apply(events);
            _version += events.Count;
        }

        public List<Event> GetChanges()
        {
            return _changes;
        }

        public long GetVersion()
        {
            return _version;
        }

        public void MarkAsCommitted()
        {
            _version += _changes.Count;
            _changes = new List<Event>();
        }
    }
}