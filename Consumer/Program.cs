namespace Consumer
{
    using Confluent.Kafka;
    using System.IO;

    public class Program
    {
        static void Main(string[] args)
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "files");

            var config = new ConsumerConfig
            {
                BootstrapServers = "kafka:29092",
                GroupId = "qualquer coisa",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                MessageMaxBytes = 5000000
            };

            using (var consumer = new ConsumerBuilder<string, byte[]>(config).Build())
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
                            var fileName = consumerResult.Message.Key;
                            var fullPath = Path.Combine(directory, fileName);
                            File.WriteAllBytes(fullPath, consumerResult.Message.Value);
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                    consumer.Close();
                }

                consumer.Close();
            }

        }
    }
}