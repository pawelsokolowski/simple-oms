using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using position_service.Models;

namespace position_service
{
    public class PositionProcessor
    {
        private readonly ILogger<PositionProcessor> _logger;
        public List<Position> Positions { get; private set; }

        public PositionProcessor(ILogger<PositionProcessor> logger)
        {
            _logger = logger;
            Positions = new List<Position>();
        }

        public void PositionChanged(PositionRaw positionRaw)
        {
            _logger.LogInformation($"Processing positions : {positionRaw}");

            var position = Positions.FirstOrDefault(p => p.Key == positionRaw.Key);

            if (position != null)
            {
                position.Quantity = positionRaw.Quantity;
                position.CurrentPrice = positionRaw.CurrentPrice;
            }
            else
            {
                Positions.Add(new Position()
                {
                    Portfolio = positionRaw.Portfolio,
                    Ticker = positionRaw.Ticker,
                    Quantity = positionRaw.Quantity,
                    CurrentPrice = positionRaw.CurrentPrice,
                    StartMarketValue = positionRaw.Quantity * positionRaw.CurrentPrice
                });
            }
        }
    }
}
