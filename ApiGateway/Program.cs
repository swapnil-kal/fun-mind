using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("_allowAllOriginPolicy", builder => builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed((host) => true));
});

var ocelotConfiguration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.userapi.json")
    .AddJsonFile($"ocelot.userapi.{builder.Environment.EnvironmentName}.json") // Load microservice-specific config
    //.AddJsonFile("ocelot.contentapi.json")
    //.AddJsonFile($"ocelot.contentapi.{builder.Environment.EnvironmentName}.json") // Load microservice-specific config 
    .AddJsonFile("ocelot.json")  // Load default Ocelot config
.Build();

builder.Services.AddOcelot(ocelotConfiguration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
// builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors("CORSPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();

//app.UseSwaggerForOcelotUI(opt =>
//{
//    opt.PathToSwaggerGenerator = "/swagger/docs";
//});

await app.UseOcelot();

app.UseStaticFiles();
//app.UseSwaggerForOcelotUI(opt =>
//{
//    opt.DownstreamSwaggerEndPointBasePath = "/swagger/docs";
//    opt.PathToSwaggerGenerator = "/swagger/docs";
//}).UseOcelot()
//  .Wait();


app.MapGet("/", () => "Hello World!");

app.Run();
    