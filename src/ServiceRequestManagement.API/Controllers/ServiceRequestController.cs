using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServiceRequestManagement.API.Application.Commands;
using ServiceRequestManagement.API.Application.DTOs;
using ServiceRequestManagement.API.Application.Extensions;
using ServiceRequestManagement.API.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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
        /// Creates a service request.
        /// </summary>
        /// <param name="request">The request body with data to create the ServiceRequest entity.</param>
        /// <returns>200 code with the created ServiceRequestDTO.</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ServiceRequestDTO>> CreateServiceRequestAsync([FromBody]CreateServiceRequestCommand request)
        {
            var result = await _mediator.Send(request);

            return Created(nameof(CreateServiceRequestAsync), result.AsServiceRequestDTO());
        }

        /// <summary>
        /// Retrieves all service requests.
        /// </summary>
        /// <returns>200 code with a list of all ServiceRequestDTOs if any exist; otherwise, 204 code.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<IEnumerable<ServiceRequestDTO>>> RetrieveAllServiceRequestsAsync()
        {
            var result = await _mediator.Send(new QueryAllServiceRequests());

            if (result.Any())
                return Ok(result
                    .Select(serviceRequests => serviceRequests.AsServiceRequestDTO()));

            return NoContent();
        }

        /// <summary>
        /// Retrieves a single service request by the passed Id.
        /// </summary>
        /// <param name="id">The Id of the expected ServiceRequest entity.</param>
        /// <returns>200 code with the expected ServiceRequestDTO if it exists; otherwise 404 code.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ServiceRequestDTO>> RetrieveServiceRequestByIdAsync(Guid id)
        {
            var result = await _mediator.Send(new QueryServiceRequestById(id));

            if (result is null)
                return NotFound();

            return Ok(result.AsServiceRequestDTO());
        }
    }
}
