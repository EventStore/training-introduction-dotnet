// using Application.Domain.Events;
// using Application.Domain.ReadModel;
// using Application.EventSourcing;
//
// namespace Application.Application
// {
//     public class AvailableSlotsProjector: IEventHandler
//     {
//         private readonly IAvailableSlotsRepository _repository;
//
//         public AvailableSlotsProjector(IAvailableSlotsRepository repository)
//         {
//             _repository = repository;
//         }
//
//         public void Handle(Event @event)
//         {
//             if (@event is Scheduled s)
//             {
//                 _repository.Add(new AvailableSlot(s.SlotId, s.StartTime, s.Duration));
//             } else if (@event is Booked b)
//             {
//                 _repository.MarkAsUnavailable(b.SlotId);
//             }
//             else if (@event is Cancelled c)
//             {
//                 _repository.MarkAsAvailable(c.SlotId);
//             }
//         }
//     }
// }
