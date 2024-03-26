namespace CodeFirstAPI.Entities
{
    public class Genre :BaseEntity
    {
        public Genre()
        {
            BookGenres = new HashSet<BookGenre>();
        }

        public string Name { get; set; }


        public ICollection<BookGenre> BookGenres { get; set; }
    }
}
