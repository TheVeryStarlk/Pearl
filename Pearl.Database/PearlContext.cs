﻿using Microsoft.EntityFrameworkCore;
using Pearl.Database.Models;

namespace Pearl.Database;

public sealed class PearlContext : DbContext
{
    public DbSet<Group> Groups { get; set; } = null!;

    public DbSet<Message> Messages { get; set; } = null!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public PearlContext(DbContextOptions<PearlContext> options) : base(options)
    {
    }
}