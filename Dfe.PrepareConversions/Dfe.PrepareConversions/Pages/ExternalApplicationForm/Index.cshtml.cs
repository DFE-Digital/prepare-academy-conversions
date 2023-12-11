using Dfe.PrepareConversions.Data.Services;

namespace Dfe.PrepareConversions.Pages.ExternalApplicationForm;

public class IndexModel : BaseAcademyConversionProjectPageModel
{
   public IndexModel(IAcademyConversionProjectRepository repository) : base(repository)
   {
   }

   public bool HasLink => string.IsNullOrWhiteSpace(Project.ExternalApplicationFormUrl) is false;

   public bool? AplicationFormSaved => Project.ExternalApplicationFormSaved;
   public string ApplicationFormLink => Project.ExternalApplicationFormUrl;
}
