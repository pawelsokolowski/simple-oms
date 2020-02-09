﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace oms_service
{
    public class Trade
    {
        public string Ticker { get; set; }
        public string Side { get; set; }
        public string Portfolio { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"Ticker:{Ticker}, Side: {Side}, Portfolio: {Portfolio}, Qty:{Quantity}, Px:{Price}";
        }
    }
}