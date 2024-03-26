using CodeFirstAPI.Context;
using CodeFirstAPI.DTOs.AuthorDTOs;
using CodeFirstAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CodeFirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly LibraryDbContext db;

        public AuthorController(LibraryDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        [Route("GetAllAuthors")]
        public IActionResult GetAll()
        {
            List<AuthorDTO> authors = db.Authors.Select(x => new AuthorDTO { Id = x.Id, FirstName = x.FirstName, LastName = x.LastName }).ToList();
            if (authors.Count <= 0)
                return NotFound();
            else
                return Ok(authors);
        }

        [HttpPost]
        [Route("CreateAuthor")]
        public IActionResult Create(AuthorCreateDTO authorCreateDTO)
        {
            try
            {
                Author author = new Author()
                {
                    FirstName = authorCreateDTO.FirstName,
                    LastName = authorCreateDTO.LastName,
                };
                db.Authors.Add(author);
                db.SaveChanges();

                return Ok(author);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("AuthorDetail")]
        public IActionResult GetAuthor(int id)
        {
            Author author = db.Authors.FirstOrDefault(x => x.Id == id);
            if (author is null)
                return NotFound();
            else
            {
                AuthorDTO authorDTO = new AuthorDTO()
                {
                    FirstName = author.FirstName,
                    LastName = author.LastName,
                    Id = author.Id
                };
                return Ok(authorDTO);
            }
        }

        [HttpPut]
        [Route("UpdateAuthor")]
        public IActionResult Update(AuthorDTO authorDTO)
        {
            Author author = db.Authors.FirstOrDefault(x => x.Id == authorDTO.Id);
            if (author is null)
                return NotFound();
            else
            {
                try
                {
                    author.FirstName = authorDTO.FirstName;
                    author.LastName = authorDTO.LastName;
                    db.SaveChanges();
                    return Ok(author);
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpDelete]
        [Route("DeleteAuthor")]
        public IActionResult Delete(int id) 
        { 
            Author author = db.Authors.FirstOrDefault(x => x.Id == id);
            if(author is null)
                return NotFound();

            try
            {
                db.Authors.Remove(author);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
