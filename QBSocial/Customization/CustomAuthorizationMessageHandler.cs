
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace QBSocial.Customization
{
    // reference code from ak, for storing token to local storage
    // reference code from BaseAddressAuthorizationMessageHandler
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler //BaseAddressAuthorizationMessageHandler
    {

        private readonly ILocalStorageService _storage;

        public CustomAuthorizationMessageHandler(ILocalStorageService storage, IAccessTokenProvider provider, NavigationManager navigationManager)
        : base(provider, navigationManager)
        {
            _storage = storage;
            // ConfigureHandler(new[] { navigationManager.BaseUri });
            ConfigureHandler(
                authorizedUrls: new[] { "https://localhost:5051/", "https://qbss.azurewebsites.net/" });
                // scopes: new[] { "API.Access" });
        }
        protected async override Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(await _storage.ContainKeyAsync("access_token")){
                var token = await _storage.GetItemAsStringAsync("access_token");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", token);
            }

            // Console.WriteLine("MHAuthorizationMessageHandler");

            return await base.SendAsync(request, cancellationToken);
        }
    }
}

