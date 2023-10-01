namespace Producer
{
    using Producer.Service;
    using System.Threading.Tasks;

    public class Program
    {
        static async Task Main(string[] args)
        {
            var kafkaProducer = new KafkaProducerService();

            await Task.Delay(-1);
        }
    }
}
