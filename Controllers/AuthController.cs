using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Dto;
using MyWebApiApp.Iservice;
using MyWebApiApp.Models;

namespace MyWebApiApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        try
        {
            var user = await _authService.RegisterUser(request);
            if (user == null)
            {
                return BadRequest(new { message = "User with this name alread exits." });
            }
            return Ok(new
            {
                Data = user,
                message = "User Register Succesfully"
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error registering User: {e}");
            return StatusCode(500, new { message = "Error creating new user" });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        if (request.Username is null || request.Password is null)
        {
            return BadRequest("Username or password required");
        }

        var token = await _authService.LoginUser(request);
        if (token is null)
        {
            return BadRequest("Username or password is wrong");
        }

        return Ok(new
        {
            token,
            message = "User login successfull"
        });
    }


}
