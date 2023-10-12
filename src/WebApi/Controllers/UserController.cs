using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("IdClaimer")]
        public IActionResult GetUserId()
        {
            Console.WriteLine("CHEGOU AQUI");
            var userIdClaim = User.FindFirst("id");

            if (userIdClaim == null)
            {
                return BadRequest("Invalid Token");
            }
            Console.WriteLine(userIdClaim);
            Console.WriteLine(userIdClaim.Value);
            return Ok(userIdClaim.Value);
        }
    }
}
