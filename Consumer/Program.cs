namespace Consumer
{
    using Confluent.Kafka;

    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "kafka:29092",
                GroupId = "qualquer coisa",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe("cat");

                var cancellationTokenSource = new CancellationTokenSource();

                Console.CancelKeyPress += (_, e) =>
                {
                    cancellationTokenSource.Cancel();
                };
                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumerResult = consumer.Consume(cancellationTokenSource.Token);
                            var message = consumerResult.Message.Value;
                            Console.WriteLine(message);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }

                consumer.Close();
            }

        }
    }
}