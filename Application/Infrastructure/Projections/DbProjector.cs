using System.Threading.Tasks;

namespace Application.Infrastructure.Projections
{
    public class DbProjector: ISubscription
    {
        readonly Projection _projection;

        public DbProjector(
            Projection projection
        )
        {
            _projection = projection;
        }

        public async Task Project(object @event)
        {
            if (!_projection.CanHandle(@event.GetType())) return;

            await _projection.Handle(@event.GetType(), @event);
        }
    }
}
