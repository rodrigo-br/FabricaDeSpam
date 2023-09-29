namespace Consumer
{
    using Confluent.Kafka;
    using Polly;
    using System.IO;

    public class Program
    {
        static void Main(string[] args)
        {
            string directory = Path.Combine(Directory.GetCurrentDirectory(), "files");
            double timespan = 10;

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryForever(
                    retryAttempt => TimeSpan.FromSeconds(timespan),
                    (exception, timeSpan, context) =>
                    {
                        Console.WriteLine($"Erro ao se conectar ao Kafka. Tentando novamente em {timespan} segundos.");
                    });

            retryPolicy.Execute(() =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = "kafka:9092",
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
                                throw new Exception();
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
            });
        }
    }
}