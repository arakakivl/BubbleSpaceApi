using BubbleSpaceApi.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BubbleSpaceApi.Infra.Mappings;

public class AnswerMapping : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        throw new NotImplementedException();
    }
}