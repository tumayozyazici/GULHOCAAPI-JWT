using CodeFirstAPI.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFirstAPI.EntityTypeConfiguration
{
    public class BookEntityConfig :BaseEntityConfig<Book>
    {
        public override void Configure(EntityTypeBuilder<Book> builder)
        {
            base.Configure(builder);

            builder.Property(x=>x.Name).IsRequired().HasMaxLength(50);
        }
    }
}
