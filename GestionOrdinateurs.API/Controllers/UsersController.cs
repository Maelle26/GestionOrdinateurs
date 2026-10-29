using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ParcInformatiqueContext _context;

    public UsersController(ParcInformatiqueContext context)
    {
        _context = context;
    }

    // Endpoint pour récupérer tous les utilisateurs
    [HttpGet]
    public async Task<IActionResult> ObtenirTous()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    // Endpoint pour chercher un utilisateur par login
    [HttpGet("{login}")]
    public async Task<IActionResult> ObtenirParLogin(string login)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login.ToLower() == login.ToLower());
        if (user == null)
        {
            return NotFound($"Utilisateur '{login}' introuvable.");
        }
        return Ok(user);
    }
}