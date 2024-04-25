using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using System.Text;
using User.Api;
using User.Api.Constants;
using User.Api.Helpers;
using User.Api.Repositories;
using User.Api.Services;

public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var jwtTokenConfiguration = builder.Configuration.GetSection(EnvironmentConstant.JWTConfiguration).Get<JwtTokenConfiguration>();
        builder.Services.AddSingleton(jwtTokenConfiguration);

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
         })        
         .AddGoogle(googleOptions =>
         {
             googleOptions.ClientId = builder.Configuration[EnvironmentConstant.GoogleAuthenticationClientId];
             googleOptions.ClientSecret = builder.Configuration[EnvironmentConstant.GoogleAuthenticationClientSecret];
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


        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IRoleService, RoleService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IJwtAuthProvider, JwtAuthProvider>();
        builder.Services.AddScoped<IAgeGroupService, AgeGroupService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IAgeGroupCategoryService, AgeGroupCategoryService>();        

        // Add repositories to container
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        builder.Services.AddScoped<IAgeGroupRepository, AgeGroupRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        builder.Services.AddScoped<IAgeGroupCategoryRepository, AgeGroupCategoryRepository>();

        // Add Database service configurations
        builder.Services.AddDbContext<UserDbContext>((sp, contextOptionsBuilder) =>
        {
            var connectionString = builder.Configuration[EnvironmentConstant.UserDbConnectionString];
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new Exception($"The connection string {EnvironmentConstant.UserDbConnectionString} is not set.");
            }

            var sqlConnection = new MySqlConnection(connectionString);

            contextOptionsBuilder.UseMySql(sqlConnection, ServerVersion.AutoDetect(connectionString), sqlOptionsBuilder =>
            {
                sqlOptionsBuilder.EnableRetryOnFailure();
                sqlOptionsBuilder.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName);
            });

            contextOptionsBuilder.EnableDetailedErrors(builder.Configuration.GetValue(EnvironmentConstant.EnableDatabaseDetailedErrors, false));
            contextOptionsBuilder.EnableSensitiveDataLogging(builder.Configuration.GetValue(EnvironmentConstant.EnableDatabaseSensitiveLogging, false));
        });

        var app = builder.Build();


        // Run Migration
        using (var scope = app.Services.CreateScope())
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            try {
                logger.LogInformation($"Migrating database...");
                var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                db.Database.Migrate();
                logger.LogInformation($"Database migration complete.");
            }
            catch(Exception ex)
            {
                logger?.LogCritical(ex, "Failed to complete database migration. Error: " + ex.Message);
                Console.Write("Failed to complete database migration. Error: " + ex.Message);
                throw;
            }           
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"Spinoff Edutech UserAPI V1 ({app.Environment.EnvironmentName})");
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