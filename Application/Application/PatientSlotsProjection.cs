using System.Threading.Tasks;
using Application.Domain.Events;
using Application.Domain.ReadModel;
using Application.Infrastructure.Projections;

namespace Application.Domain.Service.Projections
{
    public class PatientSlotsProjection : Projection
    {
        public PatientSlotsProjection(IPatientSlotsRepository repo)
        {
            When<Scheduled>(e =>
            {
                repo.Add(new ScheduledSlot(e.SlotId, e.StartTime, e.Duration));
                return Task.CompletedTask;
            });

            When<Booked>(e =>
            {
                repo.MarkAsBooked(e.SlotId, e.PatientId);
                return Task.CompletedTask;
            });

            When<Cancelled>(e =>
            {
                repo.MarkAsCancelled(e.SlotId);
                return Task.CompletedTask;
            });
        }
    }

}
