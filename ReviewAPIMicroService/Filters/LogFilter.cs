using BusinessEntitties;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ReviewAPIMicroService.Filters
{
    public class LogFilter : IActionFilter
    {
        private readonly ILogger _logger;
        public LogFilter(ILogger<LogFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDisplayName = context.ActionDescriptor.DisplayName;
            _logger.LogInformation(LoggingEvents.ActionStarted, $"{actionDisplayName} is starting.");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (null != context.Exception) return;
            var actionDisplayName = context.ActionDescriptor.DisplayName;
            _logger.LogInformation(LoggingEvents.ActionCompletedNormally, $"{actionDisplayName} completed normally.");
        }
    }
}
