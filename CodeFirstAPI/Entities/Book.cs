namespace CodeFirstAPI.Entities
{
    public class Book : BaseEntity
    {
        public Book()
        {
            AuthorBooks = new HashSet<AuthorBook>();
            BookGenres = new HashSet<BookGenre>();
        }

        public string Name { get; set; }


        public ICollection<AuthorBook> AuthorBooks { get; set; }
        public ICollection<BookGenre> BookGenres { get; set; }
    }
}
