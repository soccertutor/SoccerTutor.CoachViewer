using SoccerTutor.CoachViewer.WebApi.Shared.Events;

namespace SoccerTutor.CoachViewer.WebApi.Application.Common.Events;

public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}