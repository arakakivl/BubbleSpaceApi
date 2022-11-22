using BubbleSpaceApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubbleSpaceApi.Infra.Mappings;

public class QuestionMapping : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        throw new NotImplementedException();
    }
}