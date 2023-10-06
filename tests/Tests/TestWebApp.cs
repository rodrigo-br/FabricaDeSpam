//namespace Tests
//{
//	public class TestWebApp
//	{
//		[Fact]
//		public async Task SeiLa()
//		{
//			var mockRegisterViewModel = new RegisterViewModel()
//			{
//				Username = "test",
//				Password = "Password!23",
//				RePassword = "Password!23",
//				Email = "test@gmail.com"
//			};
//			var controller = new WebApp.Controllers.AccountController();

//			var result = await controller.Register(mockRegisterViewModel);

//			var okResult = Assert.IsType<OkObjectResult>(result);
//			Assert.Equal("Account created", okResult.Value);
//		}
//	}
//}
