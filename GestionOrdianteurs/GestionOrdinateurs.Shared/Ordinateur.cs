using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionOrdinateurs.Shared;

[Table("Ordinateur")]
public class Ordinateur
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Marque { get; set; } = string.Empty;
    public EtatOrdinateur Etat { get; set; } = EtatOrdinateur.EnService;
    public bool Vendu { get; set; } = false;

    [DefaultValue(0)]
    public int Stock { get; set; }

    [DefaultValue(0)]
    public decimal Prix { get; set; }

    public Caracteristiques Caracteristiques { get; set; } = new();

    public List<Reparation> Reparations { get; set; } = new();
}