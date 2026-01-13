using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PlastVariantTest.Authentication;
using PlastVariantTest.Data.DB;
using PlastVariantTest.Services.Api;
using Refit;
using System.Net;
using System.Reflection;

namespace PlastVariantTest;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        var services = builder.Services;

        //builder.Services.AddAuthenticationCore();
        builder.Services.AddCascadingAuthenticationState();


        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        services.AddMudServices();
        services.AddFluxor(options =>
        {
            options.ScanAssemblies(Assembly.GetExecutingAssembly());
            #if DEBUG
            options.UseReduxDevTools();
            #endif
        });

        services.AddSingleton<DataBase>();

        builder.Services
            .AddScoped<AuthenticationHttpMessageHandler>()
            .AddSingleton<ITokenStore, InMemoryAccessTokenStore>();

        builder.Services.AddScoped<CustomAuthenticationStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());
        builder.Services.AddAuthorizationCore();

        builder.Services
            .AddHttpClient("RefreshClient", c => c.BaseAddress = new Uri("https://localhost:17001"))
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler());

        builder.Services
            .AddRefitClient<ILiraApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:17001"))
            .AddHttpMessageHandler<AuthenticationHttpMessageHandler>();

        //services
        //    .AddRefitClient<ILiraApi>()
        //    .ConfigureHttpClient(client =>
        //    {
        //        client.BaseAddress = new Uri("https://localhost:17001");
        //    });

        //var app = builder.Build();

        //app.UseAuthentication();
        //app.UseAuthorization();


        await builder.Build().RunAsync();
    }
}
