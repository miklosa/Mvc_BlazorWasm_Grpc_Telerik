using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProtoBuf.Grpc.ClientFactory;
using SharedLibrary.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddCodeFirstGrpcClient<IWeatherForecastService>(options =>
{
    // Address of grpc server
    options.Address = new Uri(builder.HostEnvironment.BaseAddress);

    // another channel options (based on best practices docs on https://docs.microsoft.com/en-us/aspnet/core/grpc/performance?view=aspnetcore-6.0)
    options.ChannelOptionsActions.Add(channelOptions =>
    {
        channelOptions.HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
    });
});

// Register the Telerik services.
builder.Services.AddTelerikBlazor();

await builder.Build().RunAsync();
