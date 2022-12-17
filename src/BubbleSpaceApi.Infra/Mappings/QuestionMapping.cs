using BubbleSpaceApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubbleSpaceApi.Infra.Mappings;

public class QuestionMapping : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(k => k.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();
    
        builder.Property(p => p.Title)
            .HasMaxLength(100)
                .IsRequired();
            
        builder.Property(p => p.Description)
            .HasMaxLength(2000)
                .IsRequired();

        builder.HasMany(q => q.Answers)
            .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId);
    }
}