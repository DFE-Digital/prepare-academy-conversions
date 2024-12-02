namespace Dfe.PrepareConversions.Configuration;

public class ApplicationInsightsOptions
{
   public const string ConfigurationSection = "ApplicationInsights";
   public string ConnectionString { get; init; }
   public string InstrumentationKey { get; init; }
   public string EnableBrowserAnalytics { get; init; }
}
