namespace Producer.Interface
{
    public interface IKafkaProducerService
    {
        Task<bool> ProduceMessageAsync(string topic, string key, byte[] value);
    }
}