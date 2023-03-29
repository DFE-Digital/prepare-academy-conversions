using Dfe.PrepareConversions.Data.Services;

namespace Dfe.PrepareConversions.Pages.AnnexB;

public class IndexModel : BaseAcademyConversionProjectPageModel
{
   public IndexModel(IAcademyConversionProjectRepository repository) : base(repository)
   {
   }

   public bool HasLink => string.IsNullOrWhiteSpace(Project.AnnexBFormUrl) is false;

   public string AnnexBLink => Project.AnnexBFormUrl;
}
