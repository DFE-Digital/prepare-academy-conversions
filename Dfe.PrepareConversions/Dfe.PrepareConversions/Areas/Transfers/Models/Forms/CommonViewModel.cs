namespace Dfe.PrepareConversions.Areas.Transfers.Models.Forms
{
    public abstract class CommonViewModel
    {
        public string Urn { get; set; }
        public bool ReturnToPreview { get; set; }
        public string OutgoingAcademyName { get; set; }
    }
}