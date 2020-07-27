using MediatR;
using Microsoft.Extensions.Logging;
using ServiceRequestManagement.API.Application.Extensions;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceRequestManagement.API.Application.Behaviors
{
    /// <summary>
    /// This class implements MediatR's IPipelineBehavior interface..
    /// </summary>
    /// <typeparam name="TRequest">The requested command or query.</typeparam>
    /// <typeparam name="TResponse">The response to the requested command or query.</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// Constructor for the logging behavior.
        /// </summary>
        /// <param name="logger"></param>
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This method handles the requesting command or query, and logs information before and after the command or query finishes. 
        /// </summary>
        /// <param name="request">The requested command or query.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="next">The next delegate or behavior in the pipeline.</param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("Handling command {CommandName} ({@Command})", request.GetGenericTypeName(), request);

            var response = await next();

            _logger.LogInformation("Command {CommandName} handled - response: {@Response}", request.GetGenericTypeName(), response);

            return response;
        }
    }
}
