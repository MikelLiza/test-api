namespace APITest.Models;

using Entities;
using Microsoft.EntityFrameworkCore;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<Item> TestItem { get; set; } = null!;
    public DbSet<User> TestUser { get; set; } = null!;
}