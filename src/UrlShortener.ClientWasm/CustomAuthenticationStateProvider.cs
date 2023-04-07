using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using UrlShortener.ClientWasm.Constants;
using UrlShortener.ClientWasm.Models;
using UrlShortener.ClientWasm.Services;

namespace UrlShortener.ClientWasm
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthenticationService _authenticationService;
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage, IAuthenticationService authenticationService)
        {
            _localStorage = localStorage;
            _authenticationService = authenticationService;
        }


        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token = await _localStorage.GetItemAsync<string>(AuthConstant.TOKEN_KEY);

            if (!string.IsNullOrWhiteSpace(token))
            {
                try
                {
                    //if (await _authenticationService.ValidCheck(token) == false)
                    //{
                    //    await _localStorage.RemoveItemAsync(AuthConstant.TOKEN_KEY);
                    //    return new AuthenticationState(_anonymous);
                    //}
                    ClaimsPrincipal user = CreteClaimsPrincipal(token);
                    return new AuthenticationState(user);
                }
                catch (Exception ex)
                {
                    await Console.Out.WriteLineAsync(ex.Message);
                    await _localStorage.RemoveItemAsync(AuthConstant.TOKEN_KEY);
                }
            }
            return new AuthenticationState(_anonymous);
        }


        public async Task UpdateAuthenticationState(JwtToken jwtToken)
        {
            string token = jwtToken.Value;
            await _localStorage.SetItemAsync(AuthConstant.TOKEN_KEY, token);
            ClaimsPrincipal? user = null;
            try
            {
                user = CreteClaimsPrincipal(token);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                await _localStorage.RemoveItemAsync(AuthConstant.TOKEN_KEY);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user ?? _anonymous)));
        }

        public async Task LogOut()
        {
            await _localStorage.RemoveItemAsync(AuthConstant.TOKEN_KEY);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }


        private ClaimsPrincipal CreteClaimsPrincipal(string token)
        {
            IEnumerable<Claim> claims = JwtToken.ExtractClaims(token);
            ClaimsIdentity identity = new ClaimsIdentity(claims, AuthConstant.AUTHSCHEME);
            return new ClaimsPrincipal(identity);
        }
    }
}
