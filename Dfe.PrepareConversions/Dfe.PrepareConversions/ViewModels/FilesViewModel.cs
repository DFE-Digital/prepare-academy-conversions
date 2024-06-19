using System;
using System.Collections.Generic;

namespace Dfe.PrepareConversions.ViewModels;

public class FilesViewModel
{
   
   public List<string> FileNames { get; set; }
   public string FilePrefixSection { get; set; }
   public string SectionName { get; set; }
   public string Urn { get; set; }
   public string DownloadUrl { get; set; }
   public Guid EntityId { get; set; }
   public string ApplicationReference { get; set; }
}
