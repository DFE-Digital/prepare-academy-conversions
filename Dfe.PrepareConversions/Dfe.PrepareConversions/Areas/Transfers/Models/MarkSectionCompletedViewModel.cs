using Microsoft.AspNetCore.Mvc;

namespace Dfe.PrepareConversions.Areas.Transfers.Models
{
    public class MarkSectionCompletedViewModel
    {
        public bool ShowIsCompleted { get; set; }
        
        [BindProperty]
        public bool IsCompleted { get; set; }
    }
}