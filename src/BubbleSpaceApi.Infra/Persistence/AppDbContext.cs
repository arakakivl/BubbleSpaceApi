using BubbleSpaceApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BubbleSpaceApi.Infra.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public DbSet<Account> Accounts => this.Accounts ?? Set<Account>();
    public DbSet<Profile> Profiles => this.Profiles ?? Set<Profile>();
    public DbSet<Question> Questions => this.Questions ?? Set<Question>();
    public DbSet<Answer> Answers => this.Answers ?? Set<Answer>();
}