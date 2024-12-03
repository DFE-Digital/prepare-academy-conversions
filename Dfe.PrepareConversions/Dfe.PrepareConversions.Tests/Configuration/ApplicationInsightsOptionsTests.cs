using Dfe.PrepareConversions.Configuration;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Configuration;

public class ApplicationInsightsOptionsTests
{
    [Fact]
    public void Configuration_section_should_be_ApplicationInsights()
    {
        Assert.Equal("ApplicationInsights", ApplicationInsightsOptions.ConfigurationSection);
    }

    [Fact]
    public void ConnectionString_should_default_to_null()
    {
        var _options = new ApplicationInsightsOptions();
        Assert.Null(_options.ConnectionString);
    }

    [Fact]
    public void InstrumentationKey_should_default_to_null()
    {
        var _options = new ApplicationInsightsOptions();
        Assert.Null(_options.InstrumentationKey);
    }

    [Fact]
    public void EnableBrowserAnalytics_should_default_to_null()
    {
        var _options = new ApplicationInsightsOptions();
        Assert.Null(_options.EnableBrowserAnalytics);
    }
}
