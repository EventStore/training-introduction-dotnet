
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Domain;
using Application.Domain.Commands;
using Application.Domain.ReadModel;
using Microsoft.AspNetCore.Mvc;
using Scheduling.Domain.Infrastructure.Commands;

namespace Application.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("slots")]
    public class SlotsController : ControllerBase
    {
        private readonly IAvailableSlotsRepository _availableSlotsRepository;
        private readonly Dispatcher _dispatcher;

        public SlotsController(IAvailableSlotsRepository availableSlotsRepository, Dispatcher dispatcher)
        {
            _availableSlotsRepository = availableSlotsRepository;
            _dispatcher = dispatcher;
        }
        
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("available/{date}")]
        public List<AvailableSlot> AvailableSlots(DateTime date)
        {
            return _availableSlotsRepository.getSlotsAvailableOn(date);
        }
        
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("schedule")]
        public Task ScheduleSlot([FromBody]ScheduleSlotRequest scheduleSlot)
        {
            return _dispatcher.Dispatch(new Schedule(scheduleSlot.StartDateTime.ToString(), scheduleSlot.StartDateTime,
                scheduleSlot.Duration));
        }
    }

    public class ScheduleSlotRequest
    {
        public string CommandId { get; set; }
        public DateTime StartDateTime { get; set; }
        public TimeSpan Duration { get; set; }
    }
}