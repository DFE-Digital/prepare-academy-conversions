namespace Dfe.PrepareConversions.Data.Models.SignificantChange;

public class SetSignificantChangeLocalAuthorityObjectionsCommand(
   bool? localAuthorityRaisedObjections,
   string localAuthorityObjectionsFurtherInformation,
   string supportingEvidenceLinkSharepointLink)
{
   public bool? LocalAuthorityRaisedObjections { get; set; } = localAuthorityRaisedObjections;
   public string LocalAuthorityObjectionsFurtherInformation { get; set; } = localAuthorityObjectionsFurtherInformation;
   public string LocalAuthoritySupportingEvidenceLinkSharepointLink { get; set; } = supportingEvidenceLinkSharepointLink;
}
