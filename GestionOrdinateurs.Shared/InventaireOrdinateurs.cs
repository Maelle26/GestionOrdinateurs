namespace GestionOrdinateurs.Shared;

public class InventaireOrdinateurs
{
    public List<Ordinateur> Ordinateurs { get; set; } = new();

    public Ordinateur? this[string nom]
    {
        get => Ordinateurs.FirstOrDefault(o => o.Nom.Equals(nom, StringComparison.OrdinalIgnoreCase));
    }

    public Ordinateur? this[int id]
    {
        get => Ordinateurs.FirstOrDefault(o => o.Id == id);
    }
}