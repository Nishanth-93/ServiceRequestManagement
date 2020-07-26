using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceRequestManagement.API.Application.Commands;
using ServiceRequestManagement.API.Application.DTOs;
using ServiceRequestManagement.API.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceRequestManagement.API.Controllers
{
    /// <summary>
    /// The API controller for the ServiceRequest entity. 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly ILogger<ServiceRequestController> _logger;
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor for the ServiceRequest endpoint.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public ServiceRequestController(ILogger<ServiceRequestController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Creates a ServiceRequest entity.
        /// </summary>
        /// <param name="request">The request body with data to create the ServiceRequest entity.</param>
        /// <returns>The created ServiceRequest entity</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceRequestDTO>> CreateServiceRequestAsync([FromBody]CreateServiceRequestCommand request)
        {
            var result = await _mediator.Send(request);

            return Created(nameof(CreateServiceRequestAsync), result.AsServiceRequestDTO());
        }
    }
}
