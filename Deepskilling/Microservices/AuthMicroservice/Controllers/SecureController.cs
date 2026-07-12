using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecureController : ControllerBase
    {
        [HttpGet("data")]
        [Authorize] // Question 2: Secure API Endpoint using JWT Bearer authentication
        public IActionResult GetSecureData()
        {
            return Ok(new { Message = "This is protected data. Authorization successful!" });
        }
    }
}
