namespace Producer.Service
{
    using Confluent.Kafka;
    using Polly;
    using Producer.Interface;

    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly IProducer<string, byte[]> _kafkaProducer;

        public KafkaProducerService()
        {
            IProducer<string, byte[]> kafkaProducer = null;
            double timeSpan = 5;
            var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryForever(
                retryAttempt => TimeSpan.FromSeconds(timeSpan),
                (exception, timeSpan, context) =>
                {
                    Console.WriteLine($"Erro ao se conectar ao Kafka. Tentando novamente em {timeSpan} segundos.");
                });

            retryPolicy.Execute(() =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "kafka:9092",
                    MessageMaxBytes = 5000000
                };
                kafkaProducer = new ProducerBuilder<string, byte[]>(config).Build();
            });
            _kafkaProducer = kafkaProducer;
        }

        public async Task<bool> ProduceMessageAsync(string topic, string key, byte[] value)
        {
            try
            {
                var message = new Message<string, byte[]>
                {
                    Key = key,
                    Value = value
                };

                await _kafkaProducer.ProduceAsync(topic, message);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}. Tente novamente em alguns segundos.");
                return false;
            }
        }
    }
}
