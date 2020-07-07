using System.Threading.Tasks;
using Application.Domain.Events;
using Application.Domain.ReadModel;
using Application.Infrastructure.Projections;

namespace Application.Domain.Service.Projections
{
    public class AvailableSlotsProjection : Projection
    {
        public AvailableSlotsProjection(IAvailableSlotsRepository repo)
        {
            When<Scheduled>(e =>
            {
                repo.Add(new AvailableSlot(e.SlotId, e.StartTime, e.Duration));
                return Task.CompletedTask;
            });

            When<Booked>(e =>
            {
                repo.MarkAsUnavailable(e.SlotId);
                return Task.CompletedTask;
            });

            When<Cancelled>(e =>
            {
                repo.MarkAsAvailable(e.SlotId);
                return Task.CompletedTask;
            });
        }
    }

}
