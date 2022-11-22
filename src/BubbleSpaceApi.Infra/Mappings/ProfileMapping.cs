using BubbleSpaceApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubbleSpaceApi.Infra.Mappings;

public class ProfileMapping : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(p => p.Username)
            .HasMaxLength(30)
                .IsRequired();

        builder.Property(p => p.Bio)
            .HasMaxLength(2000)
                .IsRequired();

        builder.HasMany(prof => prof.Questions)
            .WithOne(q => q.Profile)
                .HasForeignKey(q => q.ProfileId);

        builder.HasMany(prof => prof.Answers)
            .WithOne(a => a.Profile)
                .HasForeignKey(a => a.ProfileId);
    }
}