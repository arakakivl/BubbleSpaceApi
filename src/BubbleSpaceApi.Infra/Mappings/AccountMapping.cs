using BubbleSpaceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubbleSpaceApi.Infra.Mappings;

public class AccountMapping : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(k => k.Id);

        builder.HasOne(acc => acc.Profile)
            .WithOne(prof => prof.Account)
                .HasForeignKey<Profile>(prof => prof.AccountId);
    }
}