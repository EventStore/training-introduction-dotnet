
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
    private IPatientSlotsRepository _repository;
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

    [Fact]
    public void should_return_an_empty_list_if_the_slot_was_scheduled()
    {
        var scheduled = new Scheduled(Guid.NewGuid().ToString(), _now, _tenMinutes);
        Given(scheduled);
        Then(
            new List<PatientSlot>(),
            _repository.GetPatientSlots(_patientId)
        );
    }

    [Fact]
    public void should_return_a_slot_if_was_booked()
    {
    }

    [Fact]
    public void should_return_a_slot_if_was_cancelled()
    {
    }

    [Fact]
    public void should_allow_bo_book_previously_cancelled_slot()
    {
    }
}