using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ReviewAPIMicroService.Helpers
{
    public class ApiResponse : ActionResult
    {
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(HttpStatusCode statusCode, string message = null)
        {
            StatusCode = (int) statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }


        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }


        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case (int) HttpStatusCode.NotFound:
                    return "Resource not found";
                case (int) HttpStatusCode.InternalServerError:
                    return "An unhandled error occurred";
                default:
                    return null;
            }
        }
    }
}
