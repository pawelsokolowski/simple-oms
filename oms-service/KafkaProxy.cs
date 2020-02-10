using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace simple_oms
{
    public interface IKafkaProxy
    {
        void SendTradeAsync(Trade trade);
        void SendPositionAsync(Position position);
    }

    public class KafkaProxy : IKafkaProxy
    {
        private readonly ILogger<KafkaProxy> _logger;

        public KafkaProxy(ILogger<KafkaProxy> logger)
        {
            _logger = logger;
        }

        public async void SendTradeAsync(Trade trade)
        {
            var config = new ProducerConfig() {BootstrapServers = "localhost:9092" };

            using var p = new ProducerBuilder<Null, string>(config).Build();
            try
            {
                var tradeJson = JsonSerializer.Serialize(trade);
                var dr = await p.ProduceAsync("trades", new Message<Null, string>() {Value = tradeJson});

                _logger.LogInformation($"Delivered : {dr.Value} to {dr.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed : {trade} with the following error : {e}");
            }
        }

        public async void SendPositionAsync(Position position)
        {
            var config = new ProducerConfig() { BootstrapServers = "localhost:9092"};

            using var p = new ProducerBuilder<Null, string>(config).Build();
            try
            {
                var positionJson = JsonSerializer.Serialize(position);
                var dr = await p.ProduceAsync("positions", new Message<Null, string>() {Value = positionJson});

                _logger.LogInformation($"Delivered {dr.Value} to {dr.TopicPartitionOffset}");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Delivery failed : {position} with the following error : {e}");
            }
        }
    }
}
