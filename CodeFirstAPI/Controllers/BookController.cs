using CodeFirstAPI.Context;
using CodeFirstAPI.DTOs.BookDTOs;
using CodeFirstAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext db;

        public BookController(LibraryDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("GetAllBooks")]
        public IActionResult List()
        {
            List<BookDTO> bookDTO = db.Books.Select(x => new BookDTO
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            if (bookDTO.Count <= 0)
                return NotFound();
            else
                return Ok(bookDTO);
        }

        [HttpGet]
        [Route("BookDetail")]
        public IActionResult GetBook(int id)
        {
            Book book = db.Books.FirstOrDefault(x => x.Id == id);

            if (book == null)
                return NotFound();
            else
            {
                BookDTO bookDto = new BookDTO { Id = book.Id, Name = book.Name };
                AuthorBook authorBook = db.AuthorBooks.FirstOrDefault(x => x.BookId == book.Id);

                bookDto.Genres = db.Genres.Join(db.BookGenres, x => x.Id, y => y.GenreId, (x, y) => new { Genres = x, BookGenres = y })
                    .Where(x => x.BookGenres.BookId == book.Id)
                    .Select(x => x.Genres.Name).ToList();


                bookDto.Author = db.Authors.FirstOrDefault(x => x.Id == authorBook.AuthorId).FirstName;
                return Ok(bookDto);
            }
        }

        [HttpPost]
        [Route("CreateBook")]
        public IActionResult Create(BookCreateDTO bookCreateDTO)
        {
            try
            {
                Author author = db.Authors.FirstOrDefault(x => x.FirstName.ToLower().Equals(bookCreateDTO.Author.ToLower()));

                Book book = new Book();
                book.Name = bookCreateDTO.Name;

                ExistOrCreateGenre(bookCreateDTO, book);

                string msg = ExistAuthor(author, book);

                if (msg != null)
                    return NotFound(msg);

                db.Books.Add(book);
                db.SaveChanges();

                return CreatedAtAction("GetBook", new { id = book.Id }, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string ExistAuthor(Author? author, Book book)
        {
            if (author is null)
                return "Önce yazar bilgisini girmelisiniz";
            AuthorBook authorBook = new AuthorBook()
            {
                Author = author,
                Book = book
            };

            author.AuthorBooks.Add(authorBook);
            book.AuthorBooks.Add(authorBook);

            db.AuthorBooks.Add(authorBook);

            return null;
        }

        private void ExistOrCreateGenre(BookCreateDTO bookCreateDTO, Book book)
        {
            List<Genre> genres = db.Genres.Where(x => bookCreateDTO.Genres.Contains(x.Name)).ToList();

            List<string> genreName = db.Genres.Select(x => x.Name).ToList();

            List<string> names = bookCreateDTO.Genres.Where(x => !genreName.Contains(x)).ToList();

            List<BookGenre> toBeDeleted = db.BookGenres.Where(x=>x.BookId ==bookCreateDTO.Id).ToList();

            if (toBeDeleted.Count>0)
            {
                foreach (var item in toBeDeleted)
                {
                    db.BookGenres.Remove(item);
                }
            }
            

            foreach (var item in names)
            {
                Genre genre = new Genre()
                {
                    Name = item,
                };
                db.Genres.Add(genre);
                genres.Add(genre);
            }

            foreach (var item in genres)
            {
                BookGenre bookGenre = new BookGenre()
                {
                    Book = book,
                    Genre = item,
                };
                book.BookGenres.Add(bookGenre);
                item.BookGenres.Add(bookGenre);
                db.BookGenres.Add(bookGenre);
            }
        }

        [HttpPut]
        [Route("UpdateBook")]
        public IActionResult Update(BookCreateDTO bookCreateDTO)
        {
            Book book = db.Books.FirstOrDefault(x => x.Id == bookCreateDTO.Id);
            if (book is null)
                return NotFound();
            try
            {
                book.Name = bookCreateDTO.Name;
                ExistOrCreateGenre(bookCreateDTO, book);
                Author author = db.Authors.FirstOrDefault(x => x.FirstName.ToLower() == bookCreateDTO.Author.ToLower());

                if (author is not null)
                {
                    AuthorBook authorBook = db.AuthorBooks.FirstOrDefault(x => x.BookId == book.Id);

                    authorBook.Book = book;
                    authorBook.Author = author;
                    db.SaveChanges();

                    return Ok(book);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("DeleteBook")]
        public IActionResult Delete(int id)
        {
            Book book = db.Books.FirstOrDefault(x => x.Id == id);
            AuthorBook authorBook = db.AuthorBooks.FirstOrDefault(book => book.Id == id);
            List<BookGenre> bookGenres = db.BookGenres.Where(x => x.BookId == book.Id).ToList();

            try
            {
                foreach (var item in bookGenres)
                {
                    db.BookGenres.Remove(item);
                }

                db.AuthorBooks.Remove(authorBook);
                db.Books.Remove(book);
                db.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
