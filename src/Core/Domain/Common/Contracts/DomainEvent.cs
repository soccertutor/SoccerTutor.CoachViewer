using SoccerTutor.CoachViewer.WebApi.Shared.Events;

namespace SoccerTutor.CoachViewer.WebApi.Domain.Common.Contracts;

public abstract class DomainEvent : IEvent
{
    public DateTime TriggeredOn { get; protected set; } = DateTime.UtcNow;
}