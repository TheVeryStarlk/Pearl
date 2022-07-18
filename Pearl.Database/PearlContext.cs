using Microsoft.EntityFrameworkCore;

namespace Pearl.Database;

public sealed class PearlContext : DbContext
{
    public PearlContext(DbContextOptions<PearlContext> options) : base(options)
    {
    }
}