namespace WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("IdClaimer")]
        public IActionResult GetUserId()
        {
            var userIdClaim = User.FindFirst("id");

            if (userIdClaim == null)
            {
                return BadRequest("Invalid Token");
            }
            Console.WriteLine(userIdClaim);
            Console.WriteLine(userIdClaim.Value);
            return Ok(userIdClaim.Value);
        }

        [HttpPut]
        [Route("TopicRegister")]
        public IActionResult RegisterToTopics()
        {
            var userId = User.FindFirst("id");
            return Ok(userId);
        }
    }
}
