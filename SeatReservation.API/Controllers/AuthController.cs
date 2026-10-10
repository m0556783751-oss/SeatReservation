using Microsoft.AspNetCore.Mvc;
using SeatReservation.Core.DTOs;
using SeatReservation.Core.Interfaces;

namespace SeatReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register(
            [FromBody] RegisterRequest request, CancellationToken ct)
        {
            var response = await _authService.RegisterAsync(request, ct);
            return Created("", response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(
            [FromBody] LoginRequest request, CancellationToken ct)
        {
            var response = await _authService.LoginAsync(request, ct);
            return Ok(response);
        }
    }
}
