using ApplyToBecomeInternal.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ApplyToBecomeInternal.Extensions
{
	public static class TempDataExtensions
	{
		public static void SetNotification(this ITempDataDictionary tempData, NotificationType notificationType, string notificationTitle, string notificationMessage)
		{
			tempData["NotificationType"] = notificationType.ToString().ToLower();
			tempData["NotificationTitle"] = notificationTitle;
			tempData["NotificationMessage"] = notificationMessage;
		}
	}
}
