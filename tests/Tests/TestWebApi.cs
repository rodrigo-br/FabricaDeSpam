//namespace Tests
//{
//    using Microsoft.AspNetCore.Http;
//    using Microsoft.AspNetCore.Mvc;
//    using Moq;
//    using Producer.Interface;
//    using WebApi.Controllers;
//    using Xunit.Abstractions;

//    public class TestWebApi
//    {
//        private readonly ITestOutputHelper _output;

//        public TestWebApi(ITestOutputHelper output)
//        {
//            _output = output;
//        }

//        /// <summary>
//        /// Padrão de nome : Given_When_Then
//        /// Exemplo :
//        /// Given - Dado o UploadFile
//        /// When - Quando é um arquivo válido
//        /// Then - Retorna OK
//        /// </summary>
//        [Fact]
//        public async void UploadFile_WithValidFile_ReturnsOkResult()
//        {
//            var mockKafkaProducer = new Mock<IKafkaProducerService>();
//            string fileName = "somefile.jpg";
//            var fileData = new byte[] {1, 2, 3};
//            mockKafkaProducer.Setup(x => x.ProduceMessageAsync("cat", It.IsAny<string>(), It.IsAny<byte[]>())).ReturnsAsync(true);
//            var controller = new ProducerController(mockKafkaProducer.Object);
//            var formFile = new FormFile(new MemoryStream(fileData), 0, fileData.Length, "file", fileName);
//            var directory = Path.Combine(Directory.GetCurrentDirectory(), "files");

//            var result = await controller.UploadFile(formFile);
//            string[] files = Directory.GetFiles(directory);

//            Assert.Single(files);
//            var okResult = Assert.IsType<OkObjectResult>(result);
//            mockKafkaProducer.Verify(
//                x => x.ProduceMessageAsync("cat", It.IsAny<string>(), fileData),
//                Times.Once
//            );

//            foreach (string file in files)
//            {
//                File.SetAttributes(file, FileAttributes.Normal);
//                File.Delete(file);
//            }
//        }
//    }
//}