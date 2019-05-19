using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ReviewAPIMicroService.Helpers
{
    /// <summary>  
    /// Validation filter for model state validation.  
    /// </summary>  
    /// <seealso cref="ActionFilterAttribute" />  
    public sealed class ValidateModelStateAttribute
        : ActionFilterAttribute
    {
        /// <inheritdoc />
        /// <summary>  
        /// Action execution on any controller.  
        /// </summary>  
        /// <param name="context">Action context.</param>  
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null)
                return;

            if (!context.ModelState.IsValid)
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState);
                context.Result = new ServerValidationFailedResult(problemDetails);
            }
            base.OnActionExecuting(context);
        }
    }
}
