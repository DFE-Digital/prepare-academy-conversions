﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace Dfe.PrepareConversions.Data.Models;

public class AzureAdOptions
{
   public Guid ClientId { get; set; }
   public string ClientSecret { get; set; }
   public Guid TenantId { get; set; }
   public Guid GroupId { get; set; }
   public string ApiUrl { get; set; } = "https://graph.microsoft.com/";
   public string Authority => string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}", TenantId);
   public IEnumerable<string> Scopes => new[] { $"{ApiUrl}.default" };
}
