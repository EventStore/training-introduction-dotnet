using System;
using System.Collections.Generic;
using Application.Application;
using Application.Domain.ReadModel;
using Application.Domain.WriteModel.Events;
using Application.Infrastructure.InMemory;
using Application.Infrastructure.Projections;
using Application.Test.Test;
using Xunit;

namespace Application.Test.Domain.ReadModel;

public class PatientSlotsProjectionTest : ProjectionTest
{
    private IPatientSlotsRepository _repository = default!;
    private DateTime _now = DateTime.UtcNow;
    private TimeSpan _tenMinutes = TimeSpan.FromMinutes(10);
    private String _patientId = "patient-123";

    protected override Projection GetProjection()
    {
        _repository = new InMemoryPatientSlotsRepository();
        _repository.Clear();
        return new PatientSlotsProjection(_repository);
    }

    [Fact]
    public void should_return_an_empty_list()
    {
        Given();
        Then(
            new List<PatientSlot>(),
            _repository.GetPatientSlots(_patientId)
        );
    }

    [Fact] //
    public void should_return_an_empty_list_if_the_slot_was_scheduled()
    {
        var scheduled = new Scheduled(Guid.NewGuid().ToString(), _now, _tenMinutes);
        Given(scheduled);
        Then(
            new List<PatientSlot>(),
            _repository.GetPatientSlots(_patientId)
        );
    }

    [Fact] //empty
    public void should_return_a_slot_if_was_booked()
    {
        var scheduled = new Scheduled(Guid.NewGuid().ToString(), _now, _tenMinutes);
        var booked = new Booked(scheduled.SlotId, _patientId);
        Given(scheduled, booked);
        Then(
            new List<PatientSlot> {new PatientSlot(scheduled.SlotId, scheduled.StartTime, scheduled.Duration)},
            _repository.GetPatientSlots(_patientId)
        );
    }

    [Fact] // empty
    public void should_return_a_slot_if_was_cancelled()
    {
        var scheduled = new Scheduled(Guid.NewGuid().ToString(), _now, _tenMinutes);
        var booked = new Booked(scheduled.SlotId, _patientId);
        var cancelled = new Cancelled(scheduled.SlotId, "No longer needed");

        Given(scheduled, booked, cancelled);
        Then(
            new List<PatientSlot> {new PatientSlot(scheduled.SlotId, scheduled.StartTime, scheduled.Duration, "cancelled")},
            _repository.GetPatientSlots(_patientId)
        );
    }

    [Fact] // Remove completelu
    public void should_allow_bo_book_previously_cancelled_slot()
    {
        var patientId2 = "patient-456";

        var scheduled = new Scheduled(Guid.NewGuid().ToString(), _now, _tenMinutes);
        var booked = new Booked(scheduled.SlotId, _patientId);
        var cancelled = new Cancelled(scheduled.SlotId, "No longer needed");
        var booked2 = new Booked(scheduled.SlotId, patientId2);

        Given(scheduled, booked, cancelled, booked2);
        Then(
            new List<PatientSlot> {new PatientSlot(scheduled.SlotId, scheduled.StartTime, scheduled.Duration, "cancelled")},
            _repository.GetPatientSlots(_patientId)
        );
        Then(
            new List<PatientSlot> {new PatientSlot(scheduled.SlotId, scheduled.StartTime, scheduled.Duration, "booked")},
            _repository.GetPatientSlots(patientId2)
        );
    }
}