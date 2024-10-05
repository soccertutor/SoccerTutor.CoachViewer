namespace SoccerTutor.CoachViewer.WebApi.Infrastructure.Persistence.Configuration;

internal static class SchemaNames
{
    // TODO: figure out how to capitalize these only for Oracle
    public static string Auditing = nameof(Auditing); // "AUDITING";
    public static string Catalog = nameof(Catalog); // "CATALOG";
    public static string Identity = nameof(Identity); // "IDENTITY";
    public static string MultiTenancy = nameof(MultiTenancy); // "MULTITENANCY";

#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
    public static string dbo = nameof(dbo); // "dbo";
#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
}