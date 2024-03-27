namespace CodeFirstAPI.DTOs.BookDTOs
{
    public class BookCreateDTO :BookBaseDTO
    {
        public int Id { get; set; }
        public BookCreateDTO()
        {
            Genres = new List<string>();
        }

        public string Author { get; set; }
        public List<string> Genres { get; set; }
    }
}
