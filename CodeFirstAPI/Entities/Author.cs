namespace CodeFirstAPI.Entities
{
    public class Author :BaseEntity
    {
        public Author()
        {
            AuthorBooks = new HashSet<AuthorBook>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public ICollection<AuthorBook> AuthorBooks { get; set; }
    }
}
