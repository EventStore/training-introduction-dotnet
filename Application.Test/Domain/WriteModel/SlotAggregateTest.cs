using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Domain;
using Application.Domain.Commands;
using Application.Domain.Events;
using Application.Domain.Exceptions;
using Application.Domain.ReadModel;
using Application.Domain.Service;
using Application.Domain.WriteModel.Commands;
using Application.EventSourcing;
using Scheduling.Domain.Infrastructure.Commands;
using Scheduling.Test;
using Xunit;

namespace Application.Test
{
    public class SlotAggregateTest : AggregateTest<SlotAggregate>
    {
        private static TimeSpan _tenMinutes = TimeSpan.FromMinutes(10);
        private static DateTime _now = DateTime.UtcNow;
        private static string _slotId = _now.ToString();
        private static string _patientId = "patient-1234";

        public SlotAggregateTest()
        {
            RegisterHandlers<Handlers>();
        }

        [Fact]
        public async Task should_be_scheduled()
        {
            Given();
            await When(new Schedule(_slotId, _now, _tenMinutes));
            Then(events =>
            {
                Assert.Equal(new Scheduled(_slotId, _now, _tenMinutes), events.First());
            });
        }

        [Fact]
        public async Task  should_not_be_double_scheduled()
        {
            Given(new Scheduled(_slotId, _now, _tenMinutes));
            await When(new Schedule(_slotId, _now, _tenMinutes));
            Then<SlotAlreadyScheduledException>();
        }

        [Fact]
        public async Task  should_be_booked()
        {
            Given(new Scheduled(_slotId, _now, _tenMinutes));
            await When(new Book(_slotId, _patientId));
            Then(events =>
            {
                Assert.Equal(new Booked(_slotId, _patientId), events.First());
            });
        }


        [Fact]
        public async Task  should_not_be_booked_if_was_not_scheduled()
        {
            Given();
            await When(new Book(_slotId, _patientId));
            Then<SlotNotScheduledException>();
        }

        [Fact]
        public async Task  cant_be_double_booked()
        {
            Given(new Scheduled(_slotId, _now, _tenMinutes), new Booked(_slotId, _patientId));
            await When(new Book(_slotId, _patientId));
            Then<SlotAlreadyBookedException>();
        }

        [Fact]
        public async Task can_be_cancelled()
        {
            Given(new Scheduled(_slotId, _now, _tenMinutes), new Booked(_slotId, _patientId));
            await When(new Cancel(_slotId, "No longer needed", _now));
            Then(events =>
            {
                Assert.Equal(new Cancelled(_slotId, "No longer needed"), events.First());
            });
        }

        [Fact]
        public async Task  cancelled_slot_can_be_booked_again()
        {

            Given(new Scheduled(_slotId, _now, _tenMinutes), new Booked(_slotId, _patientId), new Cancelled(_slotId, "No longer needed"));
            await When(new Book(_slotId, _patientId));
            Then(events =>
            {
                Assert.Equal(new Booked(_slotId, _patientId), events.First());
            });
        }

        // [Fact]
        // public async Task  cant_be_cancelled_after_start_time()
        // {
        //     var scheduled = new Scheduled(_idGenerator.NextGuid(), _now.Subtract(TimeSpan.FromHours(1)), _tenMinutes);
        //     var booked = new Booked(_idGenerator.NextGuid(), scheduled.Id, RandomString());
        //
        //     Given(scheduled, booked);
        //     When(new Cancel(RandomString()));
        //     Then(new SlotAlreadyStartedException());
        // }
        //
        // [Fact]
        // public async Task  cant_be_cancelled_if_wasnt_booked()
        // {
        //     var scheduled = new Scheduled(_idGenerator.NextGuid(), _now.Subtract(TimeSpan.FromHours(1)), _tenMinutes);
        //
        //     Given(scheduled);
        //     When(new Cancel(RandomString()));
        //     Then(new SlotNotBookedException());
        // }
        //
        // protected override SlotAggregate NewInstance()
        // {
        //     return new SlotAggregate(_id, () => _idGenerator.NextGuid(), () => _now);
        // }
    }
}
