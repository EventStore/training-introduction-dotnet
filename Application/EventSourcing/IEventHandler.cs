namespace Application.EventSourcing
{
    public interface IEventHandler
    {
        void Handle(Event @event);
    }
}