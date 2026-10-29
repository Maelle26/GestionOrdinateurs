using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class OrdinateursController : ControllerBase 
{
    private readonly IOrdinateurService _service;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OrdinateursController> _logger;

    //Injection du service OrdinateurService 
    public OrdinateursController(IOrdinateurService service, ILogger<OrdinateursController> logger, IConfiguration configuration)
    {
        _service = service;
        _logger = logger;
        _configuration = configuration;

        // Abonnement aux événements 
        _service.OrdinateurAjoute += AfficherMessageAjout;
        _service.OrdinateurSupprime += AfficherMessageSuppression;
        _service.OrdinateurMisAJour += AfficherMessageMAJ;
    }

    private void AfficherMessageAjout(object? sender, OrdinateurAjouteEventArgs e)
    {
        Console.WriteLine("Un ordinateur a été ajouté : ");
    }

    private void AfficherMessageSuppression(object? sender, OrdinateurSupprimeEventArgs e)
    {
        Console.WriteLine("Un ordinateur a été supprimé.");
    }

    private void AfficherMessageMAJ(object? sender, OrdinateurMisAJourEventArgs e)
    {
        Console.WriteLine("Un ordinateur a été mis à jour.");
    }

    [HttpGet]
    public async Task<IActionResult> ObtenirTous()
    {
        var ordinateurs = await _service.ObtenirTousAsync();
        return Ok(ordinateurs);
    }

    
    [HttpGet("info")]
    public IActionResult GetInfo()
    {
        var appName = _configuration["Application:Name"];
        var appVersion = _configuration["Application:Version"];
        var info = new
        {
            Name = appName,
            Version = appVersion
        };
        return Ok(info);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenirParId(int id)
    {
        var ordinateur = await _service.ObtenirParIdAsync(id);
        if (ordinateur == null)
        {
            return NotFound();
        }
        return Ok(ordinateur);
    }

    [HttpPost]
    public async Task<IActionResult> Ajouter([FromBody] Ordinateur ordi)
    {
        await _service.AjouterAsync(ordi);
        return StatusCode(201, ordi);
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> MAJ(int id, [FromBody] Ordinateur ordi)
    {
        var result = await _service.MAJAsync(id, ordi);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Supprimer(int id)
    {
        var result = await _service.SupprimerAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("disponibles")]
    public async Task<IActionResult> ObtenirDisponibles()
    {
        var disponibles = await _service.ObtenirDisponiblesAsync();
        return Ok(disponibles);
    }

    [HttpGet("marque/{marque}")]
    public async Task<IActionResult> ObtenirParMarque(string marque)
    {
        var ordinateurs = await _service.ObtenirParMarqueAsync(marque);
        return Ok(ordinateurs);
    }

    [HttpGet("tries-par-prix")]
    public async Task<IActionResult> ObtenirTriesParPrix()
    {
        var ordiTries = await _service.ObtenirTriesParPrixDecroissantAsync();
        return Ok(ordiTries);
    }

    [HttpGet("{id}/diagnostic")]
    public async Task<IActionResult> ObtenirDiagnostic(int id)
    {
        var ordi = await _service.ObtenirParIdAsync(id);
        if (ordi == null)
        {
            return NotFound("Ordinateur introuvable.");
        }

        var diagnostic = _service.Diagnostiquer(ordi);
        return Ok(new { Ordinateur = ordi.Nom, Diagnostic = diagnostic });
    }

    [HttpGet("statistiques")]
    public IActionResult ObtenirStatistiques()
    {
        var (total, enStock, valeurStock, nbReparations) = _service.CalculerStatistiques();

        return Ok(new
        {
            TotalOrdinateurs = total,
            OrdinateursDisponibles = enStock,
            ValeurTotaleStock = valeurStock,
            NombreReparations = nbReparations
        });
    }

    [HttpGet("inventaire/{nom}")]
    public async Task<IActionResult> Chercher(string nom)
    {
        var ordis = await _service.ObtenirTousAsync();
        var inventaire = new InventaireOrdinateurs
        {
            Ordinateurs = ordis
        };

        var ordinateur = inventaire[nom];
        if (ordinateur == null)
        {
            return NotFound($"Aucun ordinateur trouvé avec le nom '{nom}'.");
        }

        return Ok(ordinateur);
    }
}