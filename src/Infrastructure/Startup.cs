using System.Reflection;
using System.Runtime.CompilerServices;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Auth;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.BackgroundJobs;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Caching;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Common;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Cors;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.FileStorage;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Localization;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Mailing;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Mapping;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Middleware;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Multitenancy;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Notifications;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.OpenApi;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Persistence;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Persistence.Initialization;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.SecurityHeaders;
using SoccerTutor.CoachViewer.WebApi.Infrastructure.Validations;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Infrastructure.Test")]

namespace SoccerTutor.CoachViewer.WebApi.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var applicationAssembly = typeof(SoccerTutor.CoachViewer.WebApi.Application.Startup).GetTypeInfo().Assembly;
        MapsterSettings.Configure();
        return services
            .AddApiVersioning()
            .AddAuth(config)
            .AddBackgroundJobs(config)
            .AddCaching(config)
            .AddCorsPolicy(config)
            .AddExceptionMiddleware()
            .AddBehaviours(applicationAssembly)
            .AddHealthCheck()
            .AddPOLocalization(config)
            .AddMailing(config)
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddMultitenancy()
            .AddNotifications(config)
            .AddOpenApiDocumentation(config)
            .AddPersistence()
            .AddRequestLogging(config)
            .AddRouting(options => options.LowercaseUrls = true)
            .AddServices();
    }

    private static IServiceCollection AddApiVersioning(this IServiceCollection services) =>
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });

    private static IServiceCollection AddHealthCheck(this IServiceCollection services) =>
        services.AddHealthChecks().AddCheck<TenantHealthCheck>("Tenant").Services;

    public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        // Create a new scope to retrieve scoped services
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabasesAsync(cancellationToken);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
        builder
            .UseRequestLocalization()
            .UseStaticFiles()
            .UseSecurityHeaders(config)
            .UseFileStorage()
            .UseExceptionMiddleware()
            .UseRouting()
            .UseCorsPolicy()
            .UseAuthentication()
            .UseCurrentUser()
            .UseMultiTenancy()
            .UseAuthorization()
            .UseRequestLogging(config)
            .UseHangfireDashboard(config)
            .UseOpenApiDocumentation(config);

    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers().RequireAuthorization();
        builder.MapHealthCheck();
        builder.MapNotifications();
        return builder;
    }

    private static IEndpointConventionBuilder MapHealthCheck(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapHealthChecks("/api/health");
}