using System;
using System.Collections.Generic;
using Application.Domain.Events;
using Application.Domain.ReadModel;
using Application.Domain.Service.Projections;
using Application.Infrastructure.InMemory;
using Application.Infrastructure.Projections;
using Application.Test.Test;
using Xunit;

namespace Application.Test.Domain.ReadModel
{
    public class AvailableSlotsProjectionTest : ProjectionTest
    {
        private static InMemoryAvailableSlotsRepository _repository;
        private DateTime _now = DateTime.UtcNow;
        private TimeSpan _tenMinutes = TimeSpan.FromMinutes(10);

        protected override Projection GetProjection()
        {
            _repository = new InMemoryAvailableSlotsRepository();
            return new AvailableSlotsProjection(_repository);
        }

        [Fact]
        public void should_add_slot_to_the_list()
        {
            var scheduled = new Scheduled(Guid.NewGuid().ToString(), _now, _tenMinutes);

            Given(scheduled);
            Then(
                new List<AvailableSlot> {new AvailableSlot(scheduled.SlotId, scheduled.StartTime, scheduled.Duration)},
                _repository.getSlotsAvailableOn(_now.Date)
            );
        }

        [Fact]
        public void should_remove_slot_from_the_list_if_was_booked()
        {
            var scheduled = new Scheduled(Guid.NewGuid().ToString(), _now, _tenMinutes);
            var booked = new Booked(scheduled.SlotId, "patient-123");

            Given(scheduled, booked);
            Then(new List<AvailableSlot>(), _repository.getSlotsAvailableOn(_now.Date));
        }

        [Fact]
        public void should_add_slot_again_if_booking_was_cancelled()
        {
            var scheduled = new Scheduled(Guid.NewGuid().ToString(), _now, _tenMinutes);
            var booked = new Booked(scheduled.SlotId, "patient-123");
            var cancelled = new Cancelled(scheduled.SlotId, "No longer needed");

            Given(scheduled, booked, cancelled);
            Then(
                new List<AvailableSlot> {new AvailableSlot(scheduled.SlotId, scheduled.StartTime, scheduled.Duration)},
                _repository.getSlotsAvailableOn(_now.Date)
            );
        }
    }
}
