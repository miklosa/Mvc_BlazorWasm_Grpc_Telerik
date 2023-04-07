using MvcApp.Services;
using Microsoft.AspNetCore.Builder;
using ProtoBuf.Grpc.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.Server;
using ProtoBuf.Grpc.ClientFactory;
using SharedLibrary.Services;
using Grpc.Net.Client.Web;
using SharedLibrary.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// GRPC Code-First
builder.Services.AddCodeFirstGrpc(config =>
{
    config.ResponseCompressionLevel = global::System.IO.Compression.CompressionLevel.Optimal;
    config.EnableDetailedErrors = true;
    config.MaxReceiveMessageSize = null; // unlimited size
});
builder.Services.TryAddSingleton(BinderConfiguration.Create(binder: new TestGrpcServiceBinder(builder.Services)));
builder.Services.AddCodeFirstGrpcReflection();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

// Extra Server side registration for the Prerendered Client
builder.Services.AddCodeFirstGrpcClient<IWeatherForecastService>(options =>
{
    // Address of grpc server
    options.Address = new Uri("https://localhost:7288");

    // another channel options (based on best practices docs on https://docs.microsoft.com/en-us/aspnet/core/grpc/performance?view=aspnetcore-6.0)
    options.ChannelOptionsActions.Add(channelOptions =>
    {
        channelOptions.HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
    });
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5011/") });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseGrpcWeb(new GrpcWebOptions() { DefaultEnabled = true });
app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    // GRPC Code-First
    endpoints.MapGrpcService<WeatherForecastService>().EnableGrpcWeb().RequireCors("AllowAll");
    endpoints.MapFallbackToFile("index.html");
});

string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

app.MapGet("/GetWeatherForecasts", () => Enumerable.Range(1, 15).Select(index => new WeatherForecast
{
    Date = DateTime.Now.AddDays(index),
    TemperatureC = Random.Shared.Next(-20, 55),
    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
}).ToList());


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//endpoints.MapGrpcService<GrowerServiceContract>();

app.Run();
