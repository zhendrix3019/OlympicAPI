using Microsoft.EntityFrameworkCore;

public class MedalsContext : DbContext
{
    public MedalsContext(DbContextOptions<MedalsContext> options) : base(options) { }

    public DbSet<Country> Countries { get; set; }
}
