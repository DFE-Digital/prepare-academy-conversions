// App Insights frontend
import { ApplicationInsights } from '@microsoft/applicationinsights-web'

const clickPluginInstance = new Microsoft.ApplicationInsights.ClickAnalyticsPlugin();
const appInsights = new ApplicationInsights({ config: {
   connectionString: window.appInsights.connectionString,
   extensions: [ clickPluginInstance ],
   extensionConfig: { [clickPluginInstance.identifier] : {
      autoCapture : true,
      dataTags: { useDefaultContentNameOrId: true }
   } },
} });

appInsights.loadAppInsights();
appInsights.setAuthenticatedUserContext(window.appInsights.authenticatedUserId, null, true);
appInsights.trackPageView();
