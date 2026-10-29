using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.Tests;

public class OrdinateurServiceTest
{

    private readonly OrdinateurService _service;

    //Création d'un contexte et d'un service pour les tests
    public OrdinateurServiceTest()
    {
        var context = CreerContexte();
        _service = new OrdinateurService(context);
    }

    //Créer un contexte
    private ParcInformatiqueContext CreerContexte() {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ParcInformatiqueContext(options);
    }

    // Test1:  pour vérifier l'ajout d'un ordinateur
    [Fact]
    public void TestAjoutOrdinateur()
    {
        using var context = CreerContexte();
        var service = new OrdinateurService(context);

        // Création d'un ordinateur à ajouter
        var ordi = new OrdinateurPortable(999, "TestPC", "Dell", 16, "i7", EtatOrdinateur.EnService, 100);
        service.Ajouter(ordi);

        // Vérification que l'ordinateur a été ajouté
        var ordinateurAjoute = service.ObtenirParId(999);
        Assert.NotNull(ordinateurAjoute);
        Assert.Equal("TestPC", ordinateurAjoute.Nom);
    }

    //Test 2: Vérifie la mise à jour d'un ordinateur
    [Fact]
    public void TestSuppressionOrdinateur()
    {
        // Création d'un contexte de test 
        using var context = CreerContexte();
        var service = new OrdinateurService(context);

        // Création d'un ordinateur à ajouter et supprimer
        var ordi = new OrdinateurPortable(1000, "TestPC2", "HP", 8, "i5", EtatOrdinateur.EnService, 50);
        service.Ajouter(ordi);

        // Suppression de l'ordinateur
        service.Supprimer(1000);

        // Vérification que l'ordinateur a été supprimé
        var ordinateurSupprime = service.ObtenirParId(1000);
        Assert.Null(ordinateurSupprime);
    }


}
