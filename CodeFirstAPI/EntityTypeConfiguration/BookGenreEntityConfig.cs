using CodeFirstAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFirstAPI.EntityTypeConfiguration
{
    public class BookGenreEntityConfig :BaseEntityConfig<BookGenre>
    {
        public override void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            base.Configure(builder);
            builder.HasOne(x=>x.Book).WithMany(x=>x.BookGenres).HasForeignKey(x=>x.BookId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Genre).WithMany(x => x.BookGenres).HasForeignKey(x => x.GenreId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
