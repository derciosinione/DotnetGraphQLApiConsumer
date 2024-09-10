using Microsoft.AspNetCore.Mvc;
using WebApiGraphQLClient.Interface;
using WebApiGraphQLClient.Request;

namespace WebApiGraphQLClient.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authService.LoginAsync(request.Email, request.Password);
        return Ok(response);
    }
}