using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace simple_oms
{
    public interface ITradeProcessor
    {
        public List<Position> Positions { get; }
        public Task AddTrade(Trade trade);
    }
    public class TradeProcessor : ITradeProcessor
    {
        private readonly IRedisCacheClient _redis;
        private readonly ILogger<TradeProcessor> _logger;

        public List<Position> Positions => GetAllPositions().GetAwaiter().GetResult().Values.ToList();

        public TradeProcessor(ILogger<TradeProcessor> logger, IRedisCacheClient redis)
        {
            _logger = logger;
            _redis = redis;
        }

        public async Task<IDictionary<string,Position>> GetAllPositions()
        {
            var keys = await _redis.Db0.SearchKeysAsync("*");
            var positions = await _redis.Db0.GetAllAsync<Position>(keys);

            return positions;
        }

        public async Task AddTrade(Trade trade)
        {
            var positions = await _redis.Db0.GetAllAsync<Position>(new string[] { trade.Key });

            Position? position = positions?.FirstOrDefault().Value;

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

                await _redis.Db0.AddAsync(position.Key, position);
            }
            else if (trade.Side.Equals("buy", StringComparison.InvariantCultureIgnoreCase)) // ignore sells if there is no position 
            {
                var newPost = new Position()
                {
                    Portfolio = trade.Portfolio,
                    Ticker = trade.Ticker,
                    Quantity = trade.Quantity,
                    CurrentPrice = trade.Price,
                    MarketValue = trade.Quantity * trade.Price
                };

                await _redis.Db0.AddAsync(newPost.Key, newPost);
            }
        }
    }
}
