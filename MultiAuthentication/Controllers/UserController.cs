using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiAuthentication.Authentication;

namespace MultiAuthentication.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    [HttpGet("jwt")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult JwtEndpoint()
    {
        return Ok("You accessed this endpoint with a Jwt.");
    }

    [HttpGet("apikey")]
    [Authorize(AuthenticationSchemes = AuthenticationConstantes.ApiKeyAuthenticationScheme)]
    public IActionResult ApiKeyEndpoint()
    {
        return Ok("You accessed this endpoint with an Api Key.");
    }

    [HttpGet("multi")]
    [Authorize(Policy = AuthenticationConstantes.MultiAuthenticationPolicy)]
    public IActionResult CommonEndpoint()
    {
        return Ok("You accessed this endpoint with a Jwt or an Api Key.");
    }
}