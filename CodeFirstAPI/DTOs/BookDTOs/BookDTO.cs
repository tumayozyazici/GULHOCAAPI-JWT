namespace CodeFirstAPI.DTOs.BookDTOs
{
    public class BookDTO :BookBaseDTO
    {
        public int Id { get; set; }
        public string Author { get; set; }

        public List<string> Genres { get; set; }
        public BookDTO()
        {
            Genres = new List<string>();
        }
    }
}
