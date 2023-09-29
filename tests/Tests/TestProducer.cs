namespace Tests
{
    using Moq;
    public class TestProducer
    {
        /// <summary>
        /// Padrão de nome : Given_When_Then
        /// Exemplo :
        /// Given - Dado o construtor
        /// When - Quando não tem Kafka
        /// Then - Não deve lançar exceção
        /// </summary>
        [Fact]
        public async void Constructor_WithoutKafka_ShouldNotThrowException()
        {
            var constructorTask = Task.Run(() => new KafkaProducerService());

            await Task.Delay(5000);

            Assert.Null(Record.Exception(constructorTask.Wait));
        }
    }
}