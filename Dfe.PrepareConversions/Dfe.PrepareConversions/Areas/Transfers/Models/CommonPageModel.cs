using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Dfe.PrepareTransfers.Web.Models
{
    public abstract class CommonPageModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Urn { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ReturnToPreview { get; set; }
        public string OutgoingAcademyUrn { get; set; }
        [BindProperty]
        public string ProjectReference { get; set; }
        [BindProperty]
        public string IncomingTrustName { get; set; }
        
        [BindProperty]
        public string IncomingTrustReferenceNumber { get; set; }
        
        [BindProperty]
        public bool IsFormAMAT { get; set; }
        
        [BindProperty]
        
        public bool? IsReadOnly  { get; set; }
        
        [BindProperty(SupportsGet = true)]
        
        public DateTime? ProjectSentToCompleteDate { get; set; }
        
    }
}