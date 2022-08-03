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
        /* Primary Keys of our domain entites */
        #region
        modelBuilder.Entity<Account>()
            .HasKey(k => k.Id);

        modelBuilder.Entity<Profile>()
            .HasKey(k => k.Id);

        modelBuilder.Entity<Question>()
            .HasKey(k => k.Id);
        
        modelBuilder.Entity<Question>()
            .Property(x => x.Id)
                .ValueGeneratedOnAdd();

        modelBuilder.Entity<Answer>()
            .HasKey(k => k.Id);
        #endregion

        /* Modifying some properties */
        #region
        modelBuilder.Entity<Profile>()
            .Property(p => p.Username)
                .HasMaxLength(30)
                    .IsRequired();
        
        modelBuilder.Entity<Profile>()
            .Property(p => p.Bio)
                .HasMaxLength(2000)
                    .IsRequired();
        
        modelBuilder.Entity<Question>()
            .Property(p => p.Title)
                .HasMaxLength(100)
                    .IsRequired();
            
        modelBuilder.Entity<Question>()
            .Property(p => p.Description)
                .HasMaxLength(2000)
                    .IsRequired();

        modelBuilder.Entity<Answer>()
            .Property(p => p.Text)
                .HasMaxLength(2000)
                    .IsRequired();
        #endregion

        /* Relationships between entities */
        #region
        modelBuilder.Entity<Account>()
            .HasOne(acc => acc.Profile)
                .WithOne(prof => prof.Account)
                    .HasForeignKey<Profile>(prof => prof.AccountId);

        modelBuilder.Entity<Profile>()  
            .HasMany(prof => prof.Questions)
                .WithOne(q => q.Profile)
                    .HasForeignKey(q => q.ProfileId);

        modelBuilder.Entity<Profile>()
            .HasMany(prof => prof.Answers)
                .WithOne(a => a.Profile)
                    .HasForeignKey(a => a.ProfileId);
        
        modelBuilder.Entity<Question>()
            .HasMany(q => q.Answers)
                .WithOne(a => a.Question)
                    .HasForeignKey(a => a.QuestionId);
        #endregion
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
}