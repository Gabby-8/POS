using Microsoft.AspNetCore.Mvc;
using POS.Application.Interfaces;

namespace POS.Server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
       private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string email, string password)
        {
            var result = await _authService.Register(email, password);
            return Ok(result);
        }
    }
}
