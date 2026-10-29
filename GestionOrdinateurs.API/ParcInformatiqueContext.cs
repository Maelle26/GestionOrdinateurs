namespace GestionOrdinateurs.API;

using GestionOrdinateurs.Shared;
using Microsoft.EntityFrameworkCore;


//Classe représentant le contexte de la base de données pour le parc informatique
public class ParcInformatiqueContext : DbContext
{

    //Collection d'utilisateurs
    public DbSet<User> Users => Set<User>();

    //Constructeur
    public ParcInformatiqueContext(DbContextOptions<ParcInformatiqueContext> options) : base(options)
    {

    }

    //Propriétés
    public DbSet<Ordinateur> Ordinateurs { get; set; }
    public DbSet<Reparation> Reparations => Set<Reparation>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=parcinfo.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ordinateur>()
            .OwnsOne(o => o.Caracteristiques);

        modelBuilder.Entity<Ordinateur>()
            .HasMany(o => o.Reparations)
            .WithOne(r => r.UnOrdinateur)
            .HasForeignKey(r => r.OrdinateurId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();
    }


}
