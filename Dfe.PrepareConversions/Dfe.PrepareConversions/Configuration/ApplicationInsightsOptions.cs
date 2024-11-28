namespace Dfe.PrepareConversions.Configuration;

public class ApplicationInsightsOptions
{
   public const string ConfigurationSection = "ApplicationInsights";
   public string ConnectionString { get; set; }
   public string InstrumentationKey { get; set; }
}
