using CodeFirstAPI.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFirstAPI.EntityTypeConfiguration
{
    public class AuthorEntityConfig :BaseEntityConfig<Author>
    {
        public override void Configure(EntityTypeBuilder<Author> builder)
        {
            base.Configure(builder);
            builder.Property(x=>x.FirstName).IsRequired();
            builder.Property(x=>x.LastName).IsRequired();
        } 
    }
}
