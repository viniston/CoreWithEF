using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReviewAPIMicroService.Helpers
{
    internal class ServerValidationFailedResult : BadRequestObjectResult
    {
        internal ServerValidationFailedResult(object error) : base(error)
        {
            this.StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}