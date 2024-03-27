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
            //Loopa girdi onu ��zmek i�in newtonsoftun paketini indirdik ve g�rmezden gel dedik k�saca...
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            //JWT ayarlar�
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,   //Uygulamada izin verilen sitelerin denetlenip denetlenmeyece�ini belirler.
                        ValidateIssuer = true,     //Belirtilen sitenin denetlenip denetlenmeyece�ini belirler
                        ValidateLifetime = true,   //Ya�am s�resi olacak m�
                        ValidateIssuerSigningKey = true,    //Token�n bize ait olup olmad��� kontrol edilecek mi
                        ValidIssuers = new string[] { builder.Configuration["JWT:Issuer"] },
                        ValidAudiences = new string[] { builder.Configuration["JWT:Audience"] },
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                        ClockSkew = TimeSpan.FromMinutes(30),   //Token �zerine ekstra s�re ekler.
                    };
                });

            //Swaggerda Token kontrol� i�in eklenmesi gerekenler
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
