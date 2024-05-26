using ArabTube.Entities.Models;
using ArabTube.Services.ControllersServices;
using ArabTube.Services.ControllersServices.AuthServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.AuthServices.Interfaces;
using ArabTube.Services.ControllersServices.CloudServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.CloudServices.Interfaces;
using ArabTube.Services.ControllersServices.CommentServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.CommentServices.Interfaces;
using ArabTube.Services.ControllersServices.EmailServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.PlaylistServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.PlaylistServices.Interfaces;
using ArabTube.Services.ControllersServices.PlaylistVideoServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.PlaylistVideoServices.Interfaces;
using ArabTube.Services.ControllersServices.SubscriptionServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.SubscriptionServices.Interfaces;
using ArabTube.Services.ControllersServices.UserServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.UserServices.Interfaces;
using ArabTube.Services.ControllersServices.VideoServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.VideoServices.Interfaces;
using ArabTube.Services.ControllersServices.WatchedVideoServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.WatchedVideoServices.Interfaces;
using ArabTube.Services.DataServices.Data;
using ArabTube.Services.DataServices.Repositories.ImplementationClasses;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace ArabTube.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                .MinimumLevel.Debug()
                .CreateLogger();

            builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(
                  builder.Configuration.GetConnectionString("DefultConnection")
                ));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVideoService, VideoService>();
            builder.Services.AddScoped<ICloudService, CloudService>();
            builder.Services.AddScoped<IPlaylistService, PlaylistService>();
            builder.Services.AddScoped<IPlaylistVideoService, PlaylistVideoService>();
            builder.Services.AddScoped<IWatchedVideoService, WatchedVideoService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ISubscriptionService , SubscriptionService>();
            
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "ArabTube api",
                    Description = "Graduation Project",
                    Contact = new OpenApiContact()
                    {
                        Name = "Arab Tube",
                        Email = "arabtube72@gmail.com",
                        Url = new Uri("https://mydomain.com")
                    }
                });

                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter the JWT Key"
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                       new OpenApiSecurityScheme()
                       {
                          Reference = new OpenApiReference()
                          {
                             Type = ReferenceType.SecurityScheme,
                             Id = "Bearer"
                          },
                          Name = "Bearer",
                          In = ParameterLocation.Header
                       },
                       new List<string>()
                    }
                });
            });

            /*builder.Services.AddCors(Option =>
            {
                Option.AddPolicy("MyPlicy", option => {
                    option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                }
                );
            });*/

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            //app.UseCors("MyPlicy");
            app.UseSerilogRequestLogging();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
