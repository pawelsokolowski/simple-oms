using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;

namespace oms_service
{
    public interface ITradeProcessor
    {
        public List<Position> Positions { get; }
        public void AddTrade(Trade trade);
    }
    public class TradeProcessor : ITradeProcessor
    {
        private readonly ILogger<TradeProcessor> _logger;
        public List<Position> Positions { get; private set; }
        public TradeProcessor(ILogger<TradeProcessor> logger)
        {
            _logger = logger;
            Positions = new List<Position>();
        }

        public void AddTrade(Trade trade)
        {
            var position = Positions.FirstOrDefault(p => p.Portfolio == trade.Portfolio && p.Ticker == trade.Ticker);

            _logger.LogInformation($"Processing trade : {trade}");
            
            if (position != null)
            {
                if (trade.Side.Equals("buy", StringComparison.InvariantCultureIgnoreCase))
                {
                    position.Quantity += trade.Quantity;
                    position.CurrentPrice = trade.Price;
                    position.MarketValue = trade.Price * position.Quantity;
                }
                else
                {
                    position.Quantity -= trade.Quantity;
                    position.CurrentPrice = trade.Price;
                    position.MarketValue = trade.Price * position.Quantity;
                }
            }
            else if(trade.Side.Equals("buy", StringComparison.InvariantCultureIgnoreCase)) // ignore sells if there is no position 
            {
                Positions.Add(new Position()
                {
                    Portfolio = trade.Portfolio, 
                    Ticker = trade.Ticker, 
                    Quantity = trade.Quantity,
                    CurrentPrice = trade.Price, 
                    MarketValue = trade.Quantity * trade.Price
                });
            }
        }
    }
}
