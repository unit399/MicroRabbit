using System.Net.Mime;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [Route("api/transfer")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferService)
        {
            _transferService = transferService ?? throw new ArgumentNullException(nameof(transferService));
        }

        [HttpGet("accounts")]
        public ActionResult<IEnumerable<TransferLog>> GetTransferLogs()
        {
            return Ok(_transferService.GetTransferLogs());
        }
    }
}