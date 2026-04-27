using IndividualsDirectory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace IndividualsDirectory.Data.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Individual> Individuals => Set<Individual>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<IndividualConnection> IndividualConnections => Set<IndividualConnection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}