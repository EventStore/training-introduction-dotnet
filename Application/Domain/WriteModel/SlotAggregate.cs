using System;
using Application.Domain.Events;
using Application.Domain.Exceptions;
using Application.Infrastructure;

namespace Application.Domain
{
    public class SlotAggregate : AggregateRoot
    {
        private bool _isBooked;
        private bool _isScheduled;
        private DateTime _startTime;

        public SlotAggregate()
        {
            Register<Booked>(When);
            Register<Cancelled>(When);
            Register<Scheduled>(When);
        }

        private void When(Scheduled scheduled)
        {
            _isScheduled = true;
            _startTime = scheduled.StartTime;
            Id = scheduled.Id;
        }

        public void Schedule(string id, DateTime startTime, TimeSpan duration)
        {
            if (_isScheduled)
            {
                throw new SlotAlreadyScheduledException();
            }

            Raise(new Scheduled(id, startTime, duration));
        }

        private void When(Cancelled obj)
        {
            _isBooked = false;
        }

        public void Cancel(string reason, DateTime cancellationTime)
        {
            if (!_isBooked)
            {
                throw new SlotNotBookedException();
            }

            if (IsStarted(cancellationTime))
            {
                throw new SlotAlreadyStartedException();
            }

            if (_isBooked && !IsStarted(cancellationTime))
            {
                Raise(new Cancelled(Id, reason));
            }
        }

        private bool IsStarted(DateTime cancellationTime)
        {
            return cancellationTime.CompareTo(_startTime) > 0;
        }


        private void When(Booked booked)
        {
            _isBooked = true;
        }

        public void Book(string patientId)
        {
            if (!_isScheduled)
            {
                throw new SlotNotScheduledException();
            }

            if (_isBooked)
            {
                throw new SlotAlreadyBookedException();
            }

            Raise(new Booked(Id, patientId));
        }
    }
}
