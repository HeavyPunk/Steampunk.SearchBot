using Microsoft.EntityFrameworkCore;
using SearchBot.Lib.Appenders.Database.Models;

namespace SearchBot.Lib.Appenders.Database.Context;

public sealed class FAQContext : DbContext
{
    public DbSet<FAQPair> FaqPairs { get; set; }

    public FAQContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=pgpwd4habr;"); //TODO: to settings
    }
}