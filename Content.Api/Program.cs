using Content.Api;
using Content.Api.Constants;
using Content.Api.Helpers;
using Content.Api.Repositories;
using Content.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var jwtTokenConfiguration = builder.Configuration.GetSection(EnvironmentConstant.JWTConfiguration).Get<JwtTokenConfiguration>();
        builder.Services.AddSingleton(jwtTokenConfiguration);

        var contentFileConfiguration = builder.Configuration.GetSection(EnvironmentConstant.ContentFileConfiguration).Get<ContentFileConfiguration>();
        builder.Services.AddSingleton(contentFileConfiguration);

        MappingConfiguration.Configure();

        builder.Services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
         .AddJwtBearer(x =>
         {
             x.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidIssuer = jwtTokenConfiguration.Issuer,
                 ValidAudience = jwtTokenConfiguration.Audience,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenConfiguration.Key)),
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true
             };
         });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(EnvironmentConstant.AllowAllOriginPolicy,
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        // Add services to the container.
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        // builder.Services.AddSwaggerGen();

        builder.Services.AddSwaggerGen(config =>
        {
            config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            config.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IClaimsProvider, HttpContextClaimsProvider>();        
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
        builder.Services.AddScoped<IKidsContentService, KidsContentService>();
        

        // Add repositories to container
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
        builder.Services.AddScoped<IKidsContentRepository, KidsContentRepository>();

        // Add Database service configurations
        builder.Services.Configure<ContentDbConfiguration>(builder.Configuration.GetSection("ConnectionStrings"));
        builder.Services.AddSingleton(c =>
        {
            var options = c.GetService<IOptions<ContentDbConfiguration>>();

            return new MongoClient(options.Value.ContentDbConnection);
        });

        builder.Services.AddSingleton(c =>
        {
            var options = c.GetService<IOptions<ContentDbConfiguration>>();
            var client = c.GetService<MongoClient>();

            return client.GetDatabase(options.Value.DatabaseName);
        });
       
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Spinoff Edutech Content API V1 ({app.Environment.EnvironmentName})");
                c.DisplayRequestDuration();
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}