namespace GestionOrdinateurs.API;

using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;

// Classe permettant les opérations CRUD
public class OrdinateurService : IOrdinateurService
{
    private readonly ParcInformatiqueContext _context;
    private readonly JournalService _journalService;
    public event EventHandler<OrdinateurMisAJourEventArgs>? OrdinateurMisAJour;
    public event EventHandler<OrdinateurAjouteEventArgs>? OrdinateurAjoute;
    public event EventHandler<OrdinateurSupprimeEventArgs>? OrdinateurSupprime;


    // Constructeur
    public OrdinateurService(ParcInformatiqueContext context, JournalService journalService)
    {
        _context = context;
        _journalService = journalService;
    }

    // Méthode pour avoir l'ordinateur par son id
    public Ordinateur? ObtenirParId(int id)
    {
        return _context.Ordinateurs.Find(id);
    }

    // Méthode pour avoir tous les ordinateurs
    public List<Ordinateur> ObtenirTous()
    {
        return _context.Ordinateurs.ToList();
    }

    // Méthode pour ajouter un ordinateur à la liste
    public void Ajouter(Ordinateur ordinateur)
    {
        _context.Ordinateurs.Add(ordinateur);
        _context.SaveChanges();

        //Déclenchelement de l'événement "Ajouter"
        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs ());
    }

    // Méthode pour supprimer un ordinateur de la liste
    public bool Supprimer(int id)
    {
        var ordinateur = ObtenirParId(id);
        if (ordinateur == null)
        {
            return false;
        }
        _context.Ordinateurs.Remove(ordinateur);
        _context.SaveChanges();

        //Déclenchement de l'événement "Supprimer"
        OrdinateurSupprime?.Invoke(this, new OrdinateurSupprimeEventArgs());

        return true;
    }

    // Méthode pour mettre à jour un ordinateur donnée
    public bool MAJ(int id, Ordinateur ordinateurModifie)
    {
        var ordinateur = ObtenirParId(id);
        if (ordinateur == null)
        {
            return false;
        }
        ordinateur.Nom = ordinateurModifie.Nom;
        ordinateur.Marque = ordinateurModifie.Marque;
        ordinateur.Caracteristiques = ordinateurModifie.Caracteristiques;
        _context.SaveChanges();

        //Déclenchement de l'événement "MAJ"
        OrdinateurMisAJour?.Invoke(this, new OrdinateurMisAJourEventArgs());

        return true;
    }

    //Méthodes asynchrones
    public async Task<List<Ordinateur>> ObtenirTousAsync()
    {
        return await _context.Ordinateurs.ToListAsync();
    }

    public async Task<Ordinateur?> ObtenirParIdAsync(int id)
    {
        var ordi = await _context.Ordinateurs.FindAsync(id);
        return ordi;
    }

    public async Task<bool> AjouterAsync(Ordinateur ordinateur)
    {
        await _context.Ordinateurs.AddAsync(ordinateur);
        await _context.SaveChangesAsync();

        OrdinateurAjoute?.Invoke(this, new OrdinateurAjouteEventArgs());

        _journalService.EcrireAjout($"{ordinateur.Nom} ajouté");
        return true;
    }

    public async Task<bool> SupprimerAsync(int id)
    {
        var ordinateur = await ObtenirParIdAsync(id);
        if (ordinateur == null)
        {
            return false;
        }
        _context.Ordinateurs.Remove(ordinateur);
        await _context.SaveChangesAsync();

        OrdinateurSupprime?.Invoke(this, new OrdinateurSupprimeEventArgs());

        _journalService.EcrireSuppression($"{ordinateur.Nom} supprimé");
        return true;
    }

    public async Task<bool> MAJAsync(int id, Ordinateur ordinateurModifie)
    {
        var ordinateur = await ObtenirParIdAsync(id);
        if (ordinateur == null)
        {
            return false;
        }
        ordinateur.Nom = ordinateurModifie.Nom;
        ordinateur.Marque = ordinateurModifie.Marque;
        ordinateur.Caracteristiques = ordinateurModifie.Caracteristiques;
        await _context.SaveChangesAsync();

        OrdinateurMisAJour?.Invoke(this, new OrdinateurMisAJourEventArgs());

        _journalService.EcrireMAJ($"{ordinateur.Nom} mis à jour");
        return true;
    }

    public async Task<List<Ordinateur>> ObtenirDisponiblesAsync()
    {
        return await _context.Ordinateurs
            .Where(o => o.Stock > 0)
            .ToListAsync();
    }

    public async Task<List<Ordinateur>> ObtenirParMarqueAsync(string marque)
    {
        return await _context.Ordinateurs
            .Where(o => o.Marque.Equals(marque, StringComparison.OrdinalIgnoreCase))
            .ToListAsync();
    }

    public async Task<List<Ordinateur>> ObtenirTriesParPrixDecroissantAsync()
    {
        return await _context.Ordinateurs
            .OrderByDescending(o => o.Prix)
            .ToListAsync();
    }

    public string Diagnostiquer(Ordinateur ordinateur)
    {
        if (ordinateur.Etat == EtatOrdinateur.EnPanne)
        {
            return "L'ordinateur est en panne.";
        }
        else if (ordinateur.Etat == EtatOrdinateur.HorsService)
        {
            return "L'ordinateur est hors service.";
        }
        else if (ordinateur.Etat == EtatOrdinateur.Lent)
        {
            return "L'ordinateur est lent. ";
        }
        else if (ordinateur.Etat == EtatOrdinateur.Reparable)
        {
            return "L'ordinateur est réparable.";
        }
        else
        {
            return "L'ordinateur est en bon état de fonctionnement.";
        }
    }

    public (int TotalOrdinateur, int OrdiDisponibles, decimal ValeurStock, int NbReparations) CalculerStatistiques()
    {
        int total = _context.Ordinateurs.Count();
        int disponibles = _context.Ordinateurs.Count(o => o.Stock > 0);
        decimal valeur = _context.Ordinateurs.Sum(o => o.Prix * o.Stock);
        int reparations = _context.Reparations.Count();

        return (total, disponibles, valeur, reparations);
    }
    
}