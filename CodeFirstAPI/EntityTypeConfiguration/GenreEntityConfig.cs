using CodeFirstAPI.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFirstAPI.EntityTypeConfiguration
{
    public class GenreEntityConfig :BaseEntityConfig<Genre>
    {
        public override void Configure(EntityTypeBuilder<Genre> builder)
        {
            base.Configure(builder);
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(50);

        }
    }
}
