using CodeFirstApiUI.Models.VMs.AuthorVMs;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirstApiUI.Controllers
{
    public class AuthorController : Controller
    {
        private readonly string baseAdress = "https://localhost:7185/api/Author/";

        public IActionResult Index()
        {
            List<AuthorVM> authorVMs;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAdress);
                var response = client.GetAsync("GetAllAuthors");
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var read = result.Content.ReadFromJsonAsync<List<AuthorVM>>();
                    authorVMs = read.Result;
                    return View(authorVMs);
                }
                else
                    return NotFound();
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            AuthorVM authorVM;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAdress);
                var response = client.GetAsync($"AuthorDetail?id={id}");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readAuthor = result.Content.ReadFromJsonAsync<AuthorVM>();
                    authorVM = readAuthor.Result;
                    return View(authorVM);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AuthorCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseAdress);
                    var post = client.PostAsJsonAsync<AuthorCreateVM>("CreateAuthor", vm);
                    post.Wait();
                    if (post.IsCompletedSuccessfully)
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            AuthorVM authorVM;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAdress);
                var responseAuthor = client.GetAsync($"AuthorDetail?id={id}");
                responseAuthor.Wait();
                if (!responseAuthor.IsCompletedSuccessfully)
                {
                    var readAuthor = responseAuthor.Result.Content.ReadFromJsonAsync<AuthorVM>();
                    readAuthor.Wait();
                    authorVM = readAuthor.Result;
                    return View(authorVM);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(AuthorVM authorVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(baseAdress);
                        var response = client.PutAsJsonAsync("UpdateAuthor", authorVM);
                        response.Wait();
                        if (response.IsCompletedSuccessfully)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return View(authorVM);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAdress);
                var response = client.DeleteAsync($"DeleteAuthor?id={id}");
                response.Wait();
                if (response.IsCompletedSuccessfully)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return BadRequest();
        }
    }
}
