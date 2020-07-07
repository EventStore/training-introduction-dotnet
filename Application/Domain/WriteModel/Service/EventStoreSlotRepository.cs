// using System.Threading.Tasks;
// using Scheduling.Domain.Infrastructure;
//
// namespace Application.Domain.Service
// {
//     public class EventStoreSlotRepository : ISlotRepository
//     {
//         private readonly IAggregateStore _aggregateStore;
//
//         public EventStoreSlotRepository(IAggregateStore aggregateStore)
//         {
//             _aggregateStore = aggregateStore;
//         }
//
//         public Task Save(SlotAggregate slot)
//         {
//             return _aggregateStore.Save(slot);
//         }
//
//         public Task<SlotAggregate> Get(string slotId)
//         {
//             return _aggregateStore.Load<SlotAggregate>(slotId);
//         }
//     }
// }
