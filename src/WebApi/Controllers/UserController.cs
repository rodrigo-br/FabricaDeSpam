using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("IdClaimer")]
        public IActionResult GetUserId()
        {
            var userIdClaim = User.FindFirst("id");

            if (userIdClaim == null)
            {
                return BadRequest("Invalid Token");
            }

            return Ok(userIdClaim.Value);
        }
    }
}
