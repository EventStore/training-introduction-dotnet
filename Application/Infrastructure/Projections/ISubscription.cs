using System.Threading.Tasks;

namespace Application.Infrastructure.Projections
{
    public interface ISubscription
    {
        Task Project(object @event);
    }
}
