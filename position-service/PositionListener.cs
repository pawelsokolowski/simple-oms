using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Logging;
using position_service.Models;

namespace position_service
{
    public interface IPositionListener
    {
    }

    public class PositionListener : IPositionListener
    {
        private readonly ILogger<PositionListener> _logger;
        private string _positionTopic = "positions";
        private readonly PositionProcessor _positionProcessor;

        public PositionListener(ILogger<PositionListener> logger, PositionProcessor positionProcessor)
        {
            _logger = logger;
            _positionProcessor = positionProcessor;
        }

        public void Start()
        {
            Task.Factory.StartNew(Listen);
        }

        private void Listen()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092", GroupId = "position-service",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            try
            {
                consumer.Subscribe(_positionTopic);

                while (true)
                {
                    var record = consumer.Consume();

                    _logger.LogInformation($"Received new position update : {record.Topic}:{record.Value}");

                    var position = JsonSerializer.Deserialize<PositionRaw>(record.Value);

                    _positionProcessor.PositionChanged(position);
                }
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
