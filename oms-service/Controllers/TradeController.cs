using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace simple_oms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITradeProcessor _tradeProcessor;
        private readonly ILogger<TradeController> _logger;
        public TradeController(TradeProcessor tradeProcessor, ILogger<TradeController> logger)
        {
            _tradeProcessor = tradeProcessor;
            _logger = logger;
        }

        // GET: api/Trade
        [HttpGet]
        public string Get()
        {
            return JsonSerializer.Serialize(_tradeProcessor.Positions);
        }

        // POST: api/Trade
        [HttpPost]
        public void Post([FromBody] Trade trade)
        {
            _logger.LogInformation($"Received new request : {trade}");

            if (trade != null)
            {
                _tradeProcessor.AddTradeAsync(trade);
            }
        }

        [Route("Test")]
        public string Test()
        {
            return JsonSerializer.Serialize(new Trade()
                { Portfolio = "Pawel Capital", Ticker = "IBM", Side = "Buy", Price = 12.2m, Quantity = 1234 });
        }
    }
}