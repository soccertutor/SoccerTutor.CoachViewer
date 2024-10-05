using SoccerTutor.CoachViewer.WebApi.Infrastructure.Multitenancy;

namespace SoccerTutor.CoachViewer.WebApi.Infrastructure.Persistence.Initialization;

internal interface IDatabaseInitializer
{
    Task InitializeDatabasesAsync(CancellationToken cancellationToken);
    Task InitializeApplicationDbForTenantAsync(FSHTenantInfo tenant, CancellationToken cancellationToken);
}