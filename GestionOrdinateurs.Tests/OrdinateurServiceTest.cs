using GestionOrdinateurs.Shared;
using GestionOrdinateurs.API;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace GestionOrdinateurs.Tests;

public class OrdinateurServiceTest
{
    private readonly OrdinateurService _service;
    private readonly JournalService _journalService;

    public OrdinateurServiceTest()
    {
        _journalService = new JournalService();
        var context = CreerContexte();
        _service = new OrdinateurService(context, _journalService);
    }

    private ParcInformatiqueContext CreerContexte()
    {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ParcInformatiqueContext(options);
    }

    [Fact]
    public void TestAjoutOrdinateur()
    {
        using var context = CreerContexte();
        var service = new OrdinateurService(context, _journalService);

        var ordi = new Ordinateur
        {
            Id = 999,
            Nom = "TestPC"
        };
        service.Ajouter(ordi);

        var ordinateurAjoute = service.ObtenirParId(999);
        Assert.NotNull(ordinateurAjoute);
        Assert.Equal("TestPC", ordinateurAjoute.Nom);
    }

    [Fact]
    public void TestSuppressionOrdinateur()
    {
        using var context = CreerContexte();
        var service = new OrdinateurService(context, _journalService);

        var ordi = new Ordinateur
        {
            Id = 1000,
            Nom = "TestPC2"
        };
        service.Ajouter(ordi);

        service.Supprimer(1000);

        var ordinateurSupprime = service.ObtenirParId(1000);
        Assert.Null(ordinateurSupprime);
    }
}