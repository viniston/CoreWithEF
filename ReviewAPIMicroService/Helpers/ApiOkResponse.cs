using System.Net;

namespace ReviewAPIMicroService.Helpers
{
    public class ApiOkResponse : ApiResponse
    {
        public object Result { get; }

        public ApiOkResponse(object result = null)
            : base(HttpStatusCode.OK)
        {
            Result = result;
        }
    }
}
