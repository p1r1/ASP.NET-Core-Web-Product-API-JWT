using Auth.API.Services;
using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Auth.API.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request) {
            try {
                var result = await _authService.RegisterAsync(request);
                return Ok(new { Message = result });
            }
            catch (ArgumentException ex) {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex) {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request) {
            try {
                var token = await _authService.LoginAsync(request);
                return Ok(new { Token = token });
            }
            catch (ArgumentException ex) {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex) {
                return StatusCode(500, new { Error = "An error occurred during login." });
            }
        }
    }
}