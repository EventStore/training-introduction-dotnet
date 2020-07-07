// using System;
// using System.Collections.Generic;
// using Application.Application;
// using Application.Domain.Events;
// using Application.Domain.ReadModel;
// using Application.EventSourcing;
// using Application.Infrastructure.InMemory;
// using Application.Test.Test;
// using Xunit;
//
// namespace Application.Test.Domain.ReadModel
// {
//     public class PatientSlotsProjectorTest : EventHandlerTest
//     {
//         private IPatientSlotsRepository _repository;
//         private DateTime _now = DateTime.UtcNow;
//         private TimeSpan _tenMinutes = TimeSpan.FromMinutes(10);
//         private String _patientId = "patient-123";
//
//         protected override IEventHandler EventHandler()
//         {
//             _repository = new InMemoryPatientSlotsRepository();
//             return new PatientSlotProjector(_repository);
//         }
//
//         [Fact]
//         public void should_return_an_empty_list()
//         {
//             Given();
//             Then(
//                 new List<PatientSlot>(),
//                 _repository.getPatientSlots(_patientId)
//             );
//         }
//
//         [Fact]
//         public void should_return_an_empty_list_if_the_slot_was_scheduled()
//         {
//             var scheduled = new Scheduled(_idGenerator.NextGuid(), _now, _tenMinutes);
//             Given(scheduled);
//             Then(
//                 new List<PatientSlot>(),
//                 _repository.getPatientSlots(_patientId)
//             );
//         }
//
//         [Fact]
//         public void should_return_a_slot_if_was_booked()
//         {
//             var scheduled = new Scheduled(_idGenerator.NextGuid(), _now, _tenMinutes);
//             var booked = new Booked(_idGenerator.NextGuid(), scheduled.Id, _patientId);
//             Given(scheduled, booked);
//             Then(
//                 new List<PatientSlot> {new PatientSlot(scheduled.Id, scheduled.StartTime, scheduled.Duration)},
//                 _repository.getPatientSlots(_patientId)
//             );
//         }
//
//         [Fact]
//         public void should_return_a_slot_if_was_cancelled()
//         {
//             var scheduled = new Scheduled(_idGenerator.NextGuid(), _now, _tenMinutes);
//             var booked = new Booked(_idGenerator.NextGuid(), scheduled.Id, _patientId);
//             var cancelled = new Cancelled(_idGenerator.NextGuid(), scheduled.Id, "No longer needed");
//             
//             Given(scheduled, booked, cancelled);
//             Then(
//                 new List<PatientSlot> {new PatientSlot(scheduled.Id, scheduled.StartTime, scheduled.Duration, "cancelled")},
//                 _repository.getPatientSlots(_patientId)
//             );
//         }
//
//         [Fact]
//         public void should_allow_bo_book_previously_cancelled_slot()
//         {
//             var patientId2 = "patient-456";
//             
//             var scheduled = new Scheduled(_idGenerator.NextGuid(), _now, _tenMinutes);
//             var booked = new Booked(_idGenerator.NextGuid(), scheduled.Id, _patientId);
//             var cancelled = new Cancelled(_idGenerator.NextGuid(), scheduled.Id, "No longer needed");
//             var booked2 = new Booked(_idGenerator.NextGuid(), scheduled.Id, patientId2);
//             
//             Given(scheduled, booked, cancelled, booked2);
//             Then(
//                 new List<PatientSlot> {new PatientSlot(scheduled.Id, scheduled.StartTime, scheduled.Duration, "cancelled")},
//                 _repository.getPatientSlots(_patientId)
//             );
//             Then(
//                 new List<PatientSlot> {new PatientSlot(scheduled.Id, scheduled.StartTime, scheduled.Duration, "booked")},
//                 _repository.getPatientSlots(patientId2)
//             );
//         }
//     }
// }