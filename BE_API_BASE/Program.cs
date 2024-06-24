using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BE_API_BASE.Application.Constants;
using BE_API_BASE.Application.Handle.HandleEmail;
using BE_API_BASE.Application.ImplementService;
using BE_API_BASE.Application.InterfaceService;
using BE_API_BASE.Application.Payloads.Mappers;
using BE_API_BASE.Doman.Entities;
using BE_API_BASE.Doman.InterfaceRepositories;
using BE_API_BASE.Infrastructure.DataContexts;
using BE_API_BASE.Infrastructure.ImplementRepository;

namespace BE_API_BASE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString(Constant.AppSettingKeys.DEFAULT_CONNECTION)));
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IDbContext, AppDbContext>();
            builder.Services.AddScoped<UserConverter>();
            builder.Services.AddCors();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IBaseRepository<ConfirmEmail>, BaseRepository<ConfirmEmail>>();

            var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfig);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true)
              .AllowCredentials());

            app.MapControllers();

            app.Run();
        }
    }
}
