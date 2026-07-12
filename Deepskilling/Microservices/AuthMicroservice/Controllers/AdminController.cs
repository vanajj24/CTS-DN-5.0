using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpGet("dashboard")]
        [Authorize(Roles = "Admin")] // Question 3: Secure endpoint to restrict access strictly to Admin role claims
        public IActionResult GetAdminDashboard()
        {
            return Ok(new { Message = "Welcome to the admin dashboard. Elevated privileges verified!" });
        }
    }
}
