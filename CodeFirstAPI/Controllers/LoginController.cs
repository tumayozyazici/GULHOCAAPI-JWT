using CodeFirstAPI.Context;
using CodeFirstAPI.Entities;
using CodeFirstAPI.Security;
using CodeFirstAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeFirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LibraryDbContext db;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;

        public LoginController(LibraryDbContext db, IConfiguration configuration, IUserService userService)
        {
            this.db = db;
            this.configuration = configuration;
            this.userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult authLogin(User user)
        {
            //user var mı? varsa token oluştur. Yoksa unauthorized döndür.
            bool isUser = ControlUser(user.UserName,user.Password);
            if (isUser)
            {
                Token token = TokenHandler.CreateToken(user,configuration);
                return Ok(token);
            }
            else
                return Unauthorized();
        }

        [HttpGet, Authorize]
        public IActionResult GetMe()
        {
            //var userName = User.Identity.Name;
            //var userName = User.FindFirstValue(ClaimTypes.Role);

            var user = userService.GetMyName();
            return Ok(user);
        }

        private bool ControlUser(string userName, string password)
        {
            bool result = db.Users.Any(x => x.UserName == userName && x.Password == password);
             
            return result;
        }
    }
}
