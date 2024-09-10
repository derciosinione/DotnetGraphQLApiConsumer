using Microsoft.AspNetCore.Mvc;
using WebApiGraphQLClient.Interface;
using WebApiGraphQLClient.Request;

namespace WebApiGraphQLClient.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UserController(IUserService userService) : ControllerBase
	{
		private IUserService _userService = userService;

		[HttpGet()]
		public async Task<IActionResult> GetUsers()
		{
			var response = await _userService.GetAllUsersAsync();
			return Ok(response);
		}


		[HttpGet("auth")]
		public async Task<IActionResult> GetAllUsers([FromQuery] AuthRequest request)
		{
			var response = await _userService.GetAllUsersWithAuthAsync(request.token);
			return Ok(response);
		}


		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			var response = await _userService.LoginAsync(request.Email, request.Password);
			return Ok(response);
		}

	}
}


public record AuthRequest
{
	public required string token { get; set; }
}