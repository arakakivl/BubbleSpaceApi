using BubbleSpaceApi.Domain.Entities;
using BubbleSpaceApi.Domain.Interfaces;
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

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        foreach(var entity in ChangeTracker.Entries<IDateBaseEntity>().Where(x => x.State == EntityState.Added).ToList())
            entity.Property(x => x.CreatedAt).CurrentValue = DateTimeOffset.UtcNow;

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken = default(CancellationToken));
    }

    public DbSet<Account> Accounts => this.Accounts ?? Set<Account>();
    public DbSet<Profile> Profiles => this.Profiles ?? Set<Profile>();
    public DbSet<Question> Questions => this.Questions ?? Set<Question>();
    public DbSet<Answer> Answers => this.Answers ?? Set<Answer>();
}