using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace position_service.Models
{
    public class PositionRaw
    {
        [JsonIgnore] public string Key => Portfolio + "-" + Ticker;
        public string Ticker { get; set; }
        public string Portfolio { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentPrice { get; set; }
        [JsonIgnore] public decimal MarketValue => Quantity * CurrentPrice;
    }
    public class Position
    {
        [JsonIgnore]
        public string Key => Portfolio + "-" + Ticker;
        public string Ticker { get; set; }
        public string Portfolio { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal MarketValue => Quantity * CurrentPrice;
        public decimal StartMarketValue { get; set; }
        public decimal DailyPL => MarketValue - StartMarketValue;
    }
}
