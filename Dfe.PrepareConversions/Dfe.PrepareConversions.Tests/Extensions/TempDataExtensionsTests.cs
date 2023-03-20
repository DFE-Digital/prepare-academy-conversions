using Dfe.PrepareConversions.Extensions;
using Dfe.PrepareConversions.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace Dfe.PrepareConversions.Tests.Extensions;

public class TempDataExtensionsTests
{
   [Theory]
   [InlineData(NotificationType.Success, "title", "message")]
   public void Should_set_notification_info_in_temp_data(NotificationType notificationType, string title, string message)
   {
      DefaultHttpContext httpContext = new();
      TempDataDictionary tempData = new(httpContext, Mock.Of<ITempDataProvider>());

      tempData.SetNotification(notificationType, title, message);

      tempData["NotificationType"].Should().Be(notificationType.ToString().ToLower());
      tempData["NotificationTitle"].Should().Be(title);
      tempData["NotificationMessage"].Should().Be(message);
   }
}
