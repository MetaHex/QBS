using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using QBSocial;
using QBSocial.Services;
using MudBlazor;
using MudBlazor.Services;
using Blazored.LocalStorage;
using QBSocial.Customization;

string assemblyName = "QBSocial";

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var authApiUrl = new Uri("https://localhost:5051"); //new Uri(builder.HostEnvironment.BaseAddress).ToString(); 
var noAuthApiUrl = new Uri("https://localhost:5051"); //new Uri(builder.HostEnvironment.BaseAddress).ToString();
var domain = builder.Configuration["AzureAdB2C:Domain"];
var applicationIDURI = builder.Configuration["AzureAdB2C:ApplicationIDURI"];
var scopes = builder.Configuration["AzureAdB2C:Scopes"];

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// + net 8 default
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// -

// for auth
builder.Services.AddScoped<CustomAuthorizationMessageHandler>();
builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri("https://localhost:5051"))
               .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebAPI"));

// for non auth
builder.Services.AddHttpClient("WebAPI.NoAuth",
    client => client.BaseAddress = new Uri("https://localhost:5051"));


builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add($"https://{domain}/{applicationIDURI}/{scopes}");
    //options.ProviderOptions.DefaultAccessTokenScopes.Add("https://metahexb2c.onmicrosoft.com/b48f9851-c5d4-4d8e-b85b-4948ac090804/API.Access");
});

builder.Services.AddSingleton<AppService>();
builder.Services.AddApiAuthorization(options =>
{
    options.AuthenticationPaths.LogInPath = "authentication/login";
    options.AuthenticationPaths.LogInCallbackPath = "authentication/login-callback";
    options.AuthenticationPaths.LogInFailedPath = "authentication/login-failed";
    options.AuthenticationPaths.LogOutPath = "authentication/logout";
    options.AuthenticationPaths.LogOutCallbackPath = "authentication/logout-callback";
    options.AuthenticationPaths.LogOutFailedPath = "authentication/logout-failed";
    options.AuthenticationPaths.LogOutSucceededPath = "authentication/logged-out";
    options.AuthenticationPaths.ProfilePath = "authentication/profile";
    options.AuthenticationPaths.RegisterPath = "authentication/register";
});

if (builder.HostEnvironment.IsDevelopment())
{
    builder.Services.Configure<AppSettings>(options => options.RootUrl = "https://localhost:5001/");
    builder.Services.Configure<AppSettings>(options => options.StorageUrl = "http://127.0.0.1:10000/devstoreaccount1/metahexstorage/");
    builder.Services.Configure<AppSettings>(options => options.IsProd = false);
}
else
{
    builder.Services.Configure<AppSettings>(options => options.RootUrl = "https://{account}.azurewebsites.net/"); //builder.Configuration["rootUrl"]);
    builder.Services.Configure<AppSettings>(options => options.StorageUrl = "https://{storageAccount}.blob.core.windows.net/{storageContainer}/");
    builder.Services.Configure<AppSettings>(options => options.IsProd = true);
    //
    //builder.Configuration["imgStorageUrl"]
}

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
