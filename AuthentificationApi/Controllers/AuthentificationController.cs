using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using GestionOrdinateurs.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthentificationApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public AuthenticationController(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] string login)
    {
        if (string.IsNullOrWhiteSpace(login)) return BadRequest("Login obligatoire.");

        var client = _httpClientFactory.CreateClient("GestionOrdinateursApi");
        var response = await client.GetAsync($"api/users/{login}");

        if (!response.IsSuccessStatusCode) return Unauthorized("L'utilisateur n'existe pas.");

        var json = await response.Content.ReadAsStringAsync();
        var user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (user == null) return Unauthorized("L'utilisateur n'existe pas.");

        var claims = new[]
        {
            new System.Security.Claims.Claim(ClaimTypes.Name, user.Login, System.Security.Claims.ClaimValueTypes.String),
            new System.Security.Claims.Claim(ClaimTypes.Role, user.Role.ToString(), System.Security.Claims.ClaimValueTypes.String)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return Ok(new
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            User = user.Login,
            Role = user.Role.ToString()
        });
    }
}