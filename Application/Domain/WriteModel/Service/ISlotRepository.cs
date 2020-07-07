using System.Threading.Tasks;
using Scheduling.Domain.Infrastructure;

namespace Application.Domain.Service
{
    public interface ISlotRepository
    {
        Task Save(SlotAggregate slot);

        Task<SlotAggregate> Get(string slotId);
    }
}