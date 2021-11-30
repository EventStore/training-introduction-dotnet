using System.Linq;
using Application.EventSourcing;
using Application.Infrastructure.Projections;
using Xunit;

namespace Application.Test.Test;

public abstract class ProjectionTest
{
    protected abstract Projection GetProjection();

    private Projection _projection = default!;

    protected void Given(params IEvent[] events)
    {
        _projection = GetProjection();
        events.ToList().ForEach(e => _projection.Handle(e.GetType(), e));
    }

    protected void Then(object expected, object actual)
    {
        Assert.Equal(expected, actual);
    }
}