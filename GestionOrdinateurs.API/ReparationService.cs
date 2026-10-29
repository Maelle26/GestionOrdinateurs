using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

public class ReparationService : IReparationService
{
    private readonly ParcInformatiqueContext _context;
    private readonly JournalService _journalService;

    public ReparationService(ParcInformatiqueContext context, JournalService journalService)
    {
        _context = context;
        _journalService = journalService;
    }

    public async Task SimulerReparationAsync(Ordinateur ordinateur)
    {
        var reparation = new Reparation
        {
            OrdinateurId = ordinateur.Id,
            TypeReparation = "Maintenance standard",
            DateDebut = DateTime.Now,
            Statut = "En cours",
            Technicien = "Technicien 1"
        };

        await _context.Reparations.AddAsync(reparation);
        await _context.SaveChangesAsync();

        await Task.Delay(2000);

        reparation.Statut = "Terminée";
        await _context.SaveChangesAsync();

        _journalService.EcrireReparation($"{ordinateur.Nom} réparé");
    }

    public async Task<List<Reparation>> ObtenirToutesAsync()
    {
        return await _context.Reparations
            .Include(r => r.UnOrdinateur)
            .ToListAsync();
    }

    public async Task<List<HistoriqueReparationRecord>> ObtenirHistoriqueAsync()
    {
        return await _context.Reparations
            .Include(r => r.UnOrdinateur)
            .Select(r => new HistoriqueReparationRecord(
                r.OrdinateurId,
                r.UnOrdinateur.Nom,
                r.Technicien,
                r.TypeReparation,
                r.DateDebut
            ))
            .ToListAsync();
    }
}