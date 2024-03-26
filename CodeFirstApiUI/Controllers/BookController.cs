using CodeFirstApiUI.Models.VMs.AuthorVMs;
using CodeFirstApiUI.Models.VMs.BookVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing.Text;

namespace CodeFirstApiUI.Controllers
{
    public class BookController : Controller
    {
        private readonly string baseAdress = "https://localhost:7185/api/Book/";

        [HttpGet]
        public IActionResult Index()
        {
            List<BookVM> books;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAdress);
                var response = client.GetAsync("GetAllBooks");
                response.Wait();
                if (response.IsCompletedSuccessfully)
                {
                    var readData = response.Result.Content.ReadFromJsonAsync<List<BookVM>>();
                    books = readData.Result;
                    return View(books);
                }
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAdress);
                var response = client.GetAsync($"BookDetail?id={id}");
                response.Wait();
                if (response.IsCompletedSuccessfully)
                {
                    var read = response.Result.Content.ReadFromJsonAsync<BookVM>();
                    response.Wait();
                    return View(read.Result);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            BookCreateVM createVM = new BookCreateVM();
            createVM.Authors = FillAuthors();
            return View(createVM);
        }

        private List<SelectListItem> FillAuthors()
        {
            List<AuthorVM> authors;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7185/api/Author/");
                var response = client.GetAsync("GetAllAuthors");
                response.Wait();
                if (response.IsCompletedSuccessfully)
                {
                    var read = response.Result.Content.ReadFromJsonAsync<List<AuthorVM>>();
                    read.Wait();
                    authors = read.Result;
                    return authors.Select(x => new SelectListItem()
                    {
                        Text = x.FirstName + " " + x.LastName,
                        Value = x.FirstName
                    }).ToList();
                }
                return null;
            }
        }

        [HttpPost]
        public IActionResult Create(IFormCollection collection)
        {
            BookCreateVM createVM = new BookCreateVM()
            {
                Name = collection["Name"],
                Author = collection["Author"],
                Genres = collection["type"].ToList()
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAdress);
                    var response = client.PostAsJsonAsync<BookCreateVM>("CreateBook", createVM);
                    response.Wait();
                    if (response.IsCompletedSuccessfully)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
