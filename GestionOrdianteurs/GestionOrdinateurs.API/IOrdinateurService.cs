using GestionOrdinateurs.Shared;

namespace GestionOrdinateurs.API;
public interface IOrdinateurService
{

    //Déclaration des évenements
    event EventHandler<OrdinateurAjouteEventArgs>? OrdinateurAjoute;
    event EventHandler<OrdinateurSupprimeEventArgs>? OrdinateurSupprime;
    event EventHandler<OrdinateurMisAJourEventArgs>? OrdinateurMisAJour;

    // Méthode pour avoir l'ordinateur par son id
    public Ordinateur? ObtenirParId(int id);

    // Méthode pour avoir tous les ordinateurs
    public List<Ordinateur> ObtenirTous();

    // Méthode pour ajouter un ordinateur à la liste
    public void Ajouter(Ordinateur ordinateur);

    // Méthode pour supprimer un ordinateur de la liste
    public bool Supprimer(int id);

    // Méthode pour mettre à jour un ordinateur donnée
    public bool MAJ(int id, Ordinateur ordinateurModifie);

    //Méthodes asynchrones
    public Task<List<Ordinateur>> ObtenirTousAsync();
    public Task<Ordinateur?> ObtenirParIdAsync(int id);
    public Task <bool> AjouterAsync(Ordinateur ordinateur);
    public Task<bool> SupprimerAsync(int id);
    public Task<bool> MAJAsync(int id, Ordinateur ordinateurModifie);
    public Task<List<Ordinateur>> ObtenirDisponiblesAsync();
    public Task<List<Ordinateur>> ObtenirParMarqueAsync(string marque);
    public Task<List<Ordinateur>> ObtenirTriesParPrixDecroissantAsync();
    public string Diagnostiquer(Ordinateur ordinateur);
    (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques();
}