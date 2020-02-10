using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace simple_oms
{
    public class Position
    {
        [JsonIgnore]
        public string Key => Portfolio + "-" + Ticker;
        public string Ticker { get; set; }
        public string Portfolio { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal MarketValue { get; set; }
    }
}