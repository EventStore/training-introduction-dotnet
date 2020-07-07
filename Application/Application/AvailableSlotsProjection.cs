using System.Threading.Tasks;
using Application.Domain.ReadModel;
using Application.Domain.WriteModel.Events;
using Application.Infrastructure.Projections;

namespace Application.Application
{
    public class AvailableSlotsProjection : Projection
    {
        public AvailableSlotsProjection(IAvailableSlotsRepository repo)
        {

        }
    }

}
