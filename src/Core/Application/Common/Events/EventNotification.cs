using SoccerTutor.CoachViewer.WebApi.Shared.Events;

namespace SoccerTutor.CoachViewer.WebApi.Application.Common.Events;

public class EventNotification<TEvent> : INotification
    where TEvent : IEvent
{
    public EventNotification(TEvent @event) => Event = @event;

    public TEvent Event { get; }
}