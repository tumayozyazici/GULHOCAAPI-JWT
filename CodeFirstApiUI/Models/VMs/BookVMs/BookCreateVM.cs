using Microsoft.AspNetCore.Mvc.Rendering;

namespace CodeFirstApiUI.Models.VMs.BookVMs
{
    public class BookCreateVM :BookBaseVM
    {
        public List<SelectListItem> Authors { get; set; }
        public List<string> Genres { get; set; }
        public string Author { get; set; }

        public BookCreateVM()
        {
            Genres = new List<string>();
        }
    }
}
