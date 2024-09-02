namespace Dfe.PrepareConversions.Areas.Transfers.Services.Responses

{
    public class ServiceResponseError
    {
        public ErrorCode ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public enum ErrorCode
    {
        Default = 0,
        NotFound,
        ApiError
    }
}