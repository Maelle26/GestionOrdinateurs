using Microsoft.EntityFrameworkCore;
using GestionOrdinateurs.API;
using GestionOrdinateurs.Shared;
using Xunit;

namespace GestionOrdinateurs.Tests;

public class OrdinateurServiceLinqTests
{
    private ParcInformatiqueContext ObtenirContextTest()
    {
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ParcInformatiqueContext(options);

        // Jeu de données pour les testss
        context.Ordinateurs.AddRange(
            new Ordinateur { Id = 1, Nom = "PC 1", Marque = "Dell", Stock = 3, Prix = 800m },
            new Ordinateur { Id = 2, Nom = "PC 2", Marque = "Apple", Stock = 0, Prix = 1500m },
            new Ordinateur { Id = 3, Nom = "PC 3", Marque = "Dell", Stock = 5, Prix = 600m }
        );
        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task ObtenirDisponiblesAsync_RetourneSeulementStockSuperieurAZero()
    {
        var context = ObtenirContextTest();
        var service = new OrdinateurService(context, new JournalService());

        var resultats = await service.ObtenirDisponiblesAsync();

        Assert.Equal(2, resultats.Count);
        Assert.All(resultats, o => Assert.True(o.Stock > 0));
    }

    [Fact]
    public async Task ObtenirParMarqueAsync_FiltreCorrectementParMarque()
    {
        var context = ObtenirContextTest();
        var service = new OrdinateurService(context, new JournalService());

        var resultats = await service.ObtenirParMarqueAsync("dell");

        Assert.Equal(2, resultats.Count);
        Assert.All(resultats, o => Assert.Equal("Dell", o.Marque, ignoreCase: true));
    }

    [Fact]
    public async Task ObtenirTriesParPrixDecroissantAsync_TrieDuPlusCherAuMoinsCher()
    {
        var context = ObtenirContextTest();
        var service = new OrdinateurService(context, new JournalService());

        var resultats = await service.ObtenirTriesParPrixDecroissantAsync();

        Assert.Equal(3, resultats.Count);
        Assert.Equal(1500m, resultats[0].Prix);
        Assert.Equal(800m, resultats[1].Prix);
        Assert.Equal(600m, resultats[2].Prix);
    }

    [Fact]
    public async Task CreerReparation_DoitAjouterReparationEnBase()
    {
        // Créer un contexte de test avec une BD en mémoire
        var options = new DbContextOptionsBuilder<ParcInformatiqueContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        using var context = new ParcInformatiqueContext(options);
        var ordi = new Ordinateur { Id = 1, Nom = "Test PC", Marque = "Lenovo" };
        context.Ordinateurs.Add(ordi);
        await context.SaveChangesAsync();

        var reparation = new Reparation
        {
            Id = 1,
                OrdinateurId = ordi.Id,
                Technicien = "Alice",
                TypeReparation = "Écran cassé",
                DateDebut = DateTime.Now,
                Statut = "En cours"
            };

           
            context.Reparations.Add(reparation);
            await context.SaveChangesAsync();

            
            var repEnBase = await context.Reparations.FirstOrDefaultAsync(r => r.Id == 1);
            Assert.NotNull(repEnBase);
            Assert.Equal("Alice", repEnBase.Technicien);
            Assert.Equal("Test PC", repEnBase.UnOrdinateur.Nom);
    }
}