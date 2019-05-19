using System;
using BusinessEntitties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace ReviewAPIMicroService.Filters
{
    public class CatchAllExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<CatchAllExceptionFilter> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public CatchAllExceptionFilter(ILogger<CatchAllExceptionFilter> logger,
            IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            if (IsKnownException(context.Exception) || IsKnownException(context.Exception.InnerException))
            {
                return;
            }

            // Neither the Exception or the InnerException is a known type so we need to log it.
            _logger.LogError(LoggingEvents.UnknownExceptionOccurred, context.Exception, context.Exception.Message);

        }

        private static bool IsKnownException(Exception ex)
        {
            if (null == ex)
            {
                return false;
            }

            var exceptionType = ex.GetType();
            return (exceptionType!=null);
            //return ((exceptionType == typeof(WfmDbException)) || (exceptionType == typeof(WfmServiceException)));
        }
    }
}
