

using GovUK.Dfe.CoreLibs.Contracts.ExternalApplications.Models.Response;
using GovUK.Dfe.PersonsApi.Client.Contracts;
using System.Threading.Tasks;

namespace Dfe.PrepareConversions.Data.Services.Person
{
    public interface IPersonApiEstablishmentsService
    {
      Task<Result<MemberOfParliament>> GetMemberOfParliamentBySchoolUrnAsync(int urn);
    }
}
