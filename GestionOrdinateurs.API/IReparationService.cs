using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;

public interface IReparationService
{
    Task SimulerReparationAsync(Ordinateur ordinateur);
    Task<List<Reparation>> ObtenirToutesAsync();
    Task<List<HistoriqueReparationRecord>> ObtenirHistoriqueAsync();
}