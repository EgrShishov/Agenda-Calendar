using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ErrorOr;
using System.Diagnostics;
using AgendaCalendar.WEB_API.Common.Http;
using Microsoft.Extensions.Options;

namespace AgendaCalendar.WEB_API.Common.Errors
{
    public class AgendaCalendarProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _options;

        public AgendaCalendarProblemDetailsFactory(IOptions<ApiBehaviorOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public override ProblemDetails CreateProblemDetails(
            HttpContext httpContext, 
            int? statusCode = null, 
            string? title = null, 
            string? type = null, 
            string? detail = null, 
            string? instance = null)
        {
            statusCode ??= 500;

            var problemDetails = new ProblemDetails 
            { 
                Title = title, 
                Type = type,
                Status = statusCode,
                Detail = detail,
                Instance = instance
            };

            ApplyProblemDetailsDefault(httpContext, problemDetails, statusCode.Value);
            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(
            HttpContext httpContext, 
            ModelStateDictionary modelStateDictionary, 
            int? statusCode = null, 
            string? title = null,
            string? type = null, 
            string? detail = null, 
            string? instance = null)
        {
            if(modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            statusCode ??= 400;

            var problemDetails = new ValidationProblemDetails
            {
                Type = type,
                Status = statusCode,
                Detail = detail,
                Instance = instance
            };

            if(title != null)
            {
                problemDetails.Title = title;
            }

            ApplyProblemDetailsDefault(httpContext, problemDetails, statusCode.Value);
            return problemDetails;
        }

        private void ApplyProblemDetailsDefault(
            HttpContext httpContext, 
            ProblemDetails problemDetails, 
            int statusCode)
        {
            problemDetails.Status ??= statusCode;

            if(_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
            {
                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;
            if(traceId != null)
            {
                problemDetails.Extensions["traceId"] = traceId;
            }

            var errors = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;

            if(errors is not null)
            {
                problemDetails.Extensions.Add("errorCodes", errors.Select(e => e.Code));
            }
        }
    }
}
