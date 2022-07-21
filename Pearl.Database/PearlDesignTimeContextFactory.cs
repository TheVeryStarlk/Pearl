using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pearl.Database;

public sealed class PearlDesignTimeContextFactory : IDesignTimeDbContextFactory<PearlContext>
{
    public PearlContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<PearlContext>();
        builder.UseNpgsql("Host=localhost;Database=pearl;Username=postgres;Password=123");

        return new PearlContext(builder.Options);
    }
}