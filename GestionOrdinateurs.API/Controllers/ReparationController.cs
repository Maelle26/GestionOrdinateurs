using Microsoft.AspNetCore.Mvc;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReparationController : ControllerBase
{
    private readonly IReparationService _reparService;
    private readonly IOrdinateurService _ordiService;
    private readonly StockService _stockService;

    public ReparationController(IReparationService reparService, IOrdinateurService ordiService, StockService stockService)
    {
        _reparService = reparService;
        _ordiService = ordiService;
        _stockService = stockService;
    }

    [HttpPost("simuler")]
    public async Task<IActionResult> SimulerReparation()
    {
        var listOrdinateurs = await _ordiService.ObtenirTousAsync();

        if (listOrdinateurs.Count < 3)
        {
            return BadRequest("Il faut au moins 3 ordinateurs en base pour lancer la simulation.");
        }

        await Task.WhenAll(
            _reparService.SimulerReparationAsync(listOrdinateurs[0]),
            _reparService.SimulerReparationAsync(listOrdinateurs[1]),
            _reparService.SimulerReparationAsync(listOrdinateurs[2])
        );

        return Ok("Simulation des réparations terminée.");
    }

    [HttpGet]
    public async Task<IActionResult> ObtenirToutes()
    {
        var reparations = await _reparService.ObtenirToutesAsync();
        return Ok(reparations);
    }

    [HttpGet("stock-ssd")]
    public IActionResult GetStockSSD()
    {
        Parallel.For(0, 10, i => _stockService.ConsommerSSD());
        Console.WriteLine($"Stock restant : {_stockService.StockSSD}");
        return Ok(_stockService.StockSSD);
    }

    [HttpGet("historique")]
    public async Task<IActionResult> ObtenirHistorique()
    {
        var historique = await _reparService.ObtenirHistoriqueAsync();
        return Ok(historique);
    }
}