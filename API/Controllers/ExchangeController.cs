using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeController : ControllerBase
    {
        private readonly IExchangeService _exchangeService;
        private readonly IApiKeyService _apiKeyService;

        public ExchangeController(IExchangeService exchangeService, IApiKeyService apiKeyService)
        {
            _exchangeService = exchangeService;
            _apiKeyService = apiKeyService;
        }

        [HttpGet("test")]
        public async Task<IActionResult> GetTestData()
        {
            var currency = await _exchangeService.GetTestData();
            return Ok(currency);
        }
        //http://localhost:5218/api/exchange?currencyCodes.Key=PLN&currencyCodes.Value=EUR&startDate=2009-02-01&endDate=2009-05-31
        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery] KeyValuePair<string, string> currencyCodes, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var currency = await _exchangeService.GetData(currencyCodes, startDate, endDate);
            if (currency == null)
                return NotFound();
            return Ok(currency);
        }

        [HttpPost("token")]
        public async Task<string> GenerateToken()
        {
            return await _apiKeyService.GenerateApiKeyAsync();
        }


        [Authorize]
        [HttpGet("auth")]
        public IActionResult TestAuth()
        {
            return Ok("You are authenticated!");
        }
    }
}
