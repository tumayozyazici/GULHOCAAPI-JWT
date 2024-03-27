using CodeFirstAPI.Context;
using CodeFirstAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace CodeFirstAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<LibraryDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Conn")));

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Service
            builder.Services.AddScoped<IUserService, UserService>();

            //Http
            builder.Services.AddHttpContextAccessor();
            //Loopa girdi onu çözmek için newtonsoftun paketini indirdik ve görmezden gel dedik kýsaca...
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            //JWT ayarlarý
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,   //Uygulamada izin verilen sitelerin denetlenip denetlenmeyeceðini belirler.
                        ValidateIssuer = true,     //Belirtilen sitenin denetlenip denetlenmeyeceðini belirler
                        ValidateLifetime = true,   //Yaþam süresi olacak mý
                        ValidateIssuerSigningKey = true,    //Tokenýn bize ait olup olmadýðý kontrol edilecek mi
                        ValidIssuers = new string[] { builder.Configuration["JWT:Issuer"] },
                        ValidAudiences = new string[] { builder.Configuration["JWT:Audience"] },
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                        ClockSkew = TimeSpan.FromMinutes(30),   //Token üzerine ekstra süre ekler.
                    };
                });

            //Swaggerda Token kontrolü için eklenmesi gerekenler
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Insert JWT Token",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
