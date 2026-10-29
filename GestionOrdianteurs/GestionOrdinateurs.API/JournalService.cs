namespace GestionOrdinateurs.API;

public class JournalService
{
    private void Ecrire(string sousDossier, string nomFichier, string message)
    {
        var dossier = Path.Combine("Logs", sousDossier);
        Directory.CreateDirectory(dossier);
        var chemin = Path.Combine(dossier, nomFichier);

        using var writer = new StreamWriter(chemin, append: true);
        writer.WriteLine($"{DateTime.Now:dd/MM/yyyy} - {message}");
    }

    public void EcrireReparation(string message)
    {
        Ecrire("Reparations", "reparations.log", message);
    }

    public void EcrireAjout(string message)
    {
        Ecrire("Ordinateurs", "ordinateurs.log", $"Ajout : {message}");
    }

    public void EcrireSuppression(string message)
    {
        Ecrire("Ordinateurs", "ordinateurs.log", $"Suppression : {message}");
    }

    public void EcrireMAJ(string message)
    {
        Ecrire("Ordinateurs", "ordinateurs.log", $"Modification : {message}");
    }
}