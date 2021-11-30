using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Domain;
using Application.Domain.ReadModel;
using Application.Domain.WriteModel.Commands;
using Application.Infrastructure.ES;
using EventStore.Client;
using Microsoft.AspNetCore.Mvc;
using Scheduling.Domain.Infrastructure.Commands;

namespace Application.Controllers;

[ApiController]
[Route("slots")]
public class SlotsController : ControllerBase
{
    private readonly IAvailableSlotsRepository _availableSlotsRepository;
    private readonly IPatientSlotsRepository _patientSlotRepository;

    private readonly Dispatcher _dispatcher;

    public SlotsController(
        IAvailableSlotsRepository availableSlotsRepository,
        IPatientSlotsRepository patientSlotRepository,
        Dispatcher dispatcher)
    {
        _availableSlotsRepository = availableSlotsRepository;
        _patientSlotRepository = patientSlotRepository;
        _dispatcher = dispatcher;
    }

    [HttpGet]
    [Route("available/{date}")]
    public List<AvailableSlot> AvailableSlots(DateTime date)
    {
        return _availableSlotsRepository.GetSlotsAvailableOn(date);
    }

    [HttpGet]
    [Route("my-slots/{patientId}")]
    public List<PatientSlot> MySlots(string patientId)
    {
        return _patientSlotRepository.GetPatientSlots(patientId);
    }

    [HttpPost]
    [Route("schedule")]
    public async Task<IActionResult> Schedule([FromBody] ScheduleRequest schedule)
    {
        await _dispatcher.Dispatch(new Schedule(schedule.SlotId, schedule.StartDateTime,
            schedule.Duration));

        return Ok();
    }

    [HttpPost]
    [Route("{slotId}/book")]
    public async Task<IActionResult> Book(string slotId, [FromBody] BookRequest book)
    {
        return Ok();
    }

    [HttpPost]
    [Route("{slotId}/cancel")]
    public async Task<IActionResult> Cancel(string slotId, [FromBody] CancelRequest cancel)
    {
        return Ok();
    }
}

public class ScheduleRequest
{
    public string SlotId { get; set; }
    public DateTime StartDateTime { get; set; }
    public TimeSpan Duration { get; set; }
}

public class BookRequest
{
    public string PatientId { get; set; }
}

public class CancelRequest
{
    public string Reason { get; set; }
}