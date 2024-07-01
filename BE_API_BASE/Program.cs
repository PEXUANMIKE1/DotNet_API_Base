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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

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
            builder.Services.AddScoped<IBaseRepository<ConfirmEmail>, BaseRepository<ConfirmEmail>>();
            builder.Services.AddScoped<IBaseRepository<Permission>, BaseRepository<Permission>>();
            builder.Services.AddScoped<IBaseRepository<Role>, BaseRepository<Role>>();
            builder.Services.AddScoped<IBaseRepository<RefeshToken>, BaseRepository<RefeshToken>>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IDbContext, AppDbContext>();
            builder.Services.AddScoped<UserConverter>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddCors();
            builder.Services.AddScoped<IEmailService, EmailService>();

            var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfig);
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                };
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Auth Api", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Vui lòng nhập token",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
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
