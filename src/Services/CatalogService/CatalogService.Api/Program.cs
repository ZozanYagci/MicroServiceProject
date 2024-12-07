using CatalogService.Api;
using CatalogService.Api.Extensions;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CatalogService.Api", Version = "v1" });
});


builder.Services.Configure<CatalogSettings>(builder.Configuration.GetSection("CatalogSettings"));
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureConsul(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<CatalogContextSeed>>();
    new CatalogContextSeed().SeedAsync(context, env, logger).Wait();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CatalogService.Api v1"));
}

// Serve static files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Pics")),
    RequestPath = "/pics"
});

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Consul registration
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
app.RegisterWithConsul(lifetime);

app.Run();


