using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace oms_service
{
    public class Position
    {
        public string Ticker { get; set; }
        public string Portfolio { get; set; }
        public int Quantity { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal MarketValue { get; set; }
    }
}
