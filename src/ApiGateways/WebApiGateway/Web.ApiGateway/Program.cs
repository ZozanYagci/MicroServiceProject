
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Infrastructure;
using Web.ApiGateway.Services;
using Web.ApiGateway.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("Configurations/ocelot.json");

builder.Services.AddOcelot().AddConsul();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ICatalogService, CatalogService>();
builder.Services.AddScoped<IBasketService, BasketService>();    

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy",
        builder => builder.SetIsOriginAllowed((host) => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHttpClient("basket", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["urls:basket"]!);
})
    .AddHttpMessageHandler<HttpClientDelegatingHandler>();

builder.Services.AddHttpClient("catalog", c =>
{
    c.BaseAddress = new Uri(builder.Configuration["urls:catalog"]!);
})
    .AddHttpMessageHandler<HttpClientDelegatingHandler>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthorization();


app.MapControllers();

await app.UseOcelot();

app.Run();


