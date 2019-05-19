using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ReviewAPIMicroService.Helpers
{
    public class HttpResponseCodeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult result)
            {
                var genericResponseType = typeof(ServiceResponse<>);

                var resultType = result.Value.GetType();

                if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == genericResponseType)
                {
                    // The use of dynamic here is to reduce the amount of reflection code I'd need to write
                    dynamic resultValue = result.Value;

                    if (resultValue.State == ServiceResponseState.Success)
                    {
                        context.HttpContext.Response.StatusCode = resultValue.ResultCode == 0 ? 200 : resultValue.ResultCode;
                        context.Result = new OkObjectResult(resultValue.Data);
                    }
                    else if (resultValue.State == ServiceResponseState.NotAuth)
                    {
                        context.HttpContext.Response.StatusCode = resultValue.ResultCode == 0 ? 403 : resultValue.ResultCode;
                        context.Result = new ForbidResult();
                    }
                    else
                    {
                        context.HttpContext.Response.StatusCode = 400;
                        context.Result = new BadRequestResult();
                        context.Canceled = true;
                    }
                }
            }
            base.OnActionExecuted(context);
        }
    }
}
