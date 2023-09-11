using MediatR;
using Microsoft.AspNetCore.Mvc;
using RabbitMQApp.Domain.Commands;

namespace RabbitMQApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateMessageController : ControllerBase
    {
        [HttpPost]
        public IActionResult Create([FromServices] IMediator mediator, [FromBody] CreateMessageRequest command)
        {
            mediator.Send(command);
            return Ok();
        }
    }
}