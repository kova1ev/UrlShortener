using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace UrlShortener.ClientWasm
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private const string AUTHSCHEME = "FAKE";
        private const string KEY = "authkey";
        private readonly ILocalStorageService _localStorage;

        public CustomAuthenticationStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string name = await _localStorage.GetItemAsStringAsync(KEY);
            if (!string.IsNullOrWhiteSpace(name))
            {
                var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, name), }, "Fake Auth");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            // получить из локал сторедж (токен) клаимсы 
            // если их есть UpdateAuthenticationState
            // если нету то анонимный пользователь
            // Claim claim1 = new Claim(ClaimTypes.Name, AuthConstant.ADMIN);
            //  var identity = new ClaimsIdentity(new[] { claim1 }, AUTHSCHEME);
            return new AuthenticationState(_anonymous);
        }


        public async Task UpdateAuthenticationState(string name)
        {
            // TODO :
            // получить имя(email) и пароль
            // (создать клаимсы ) проверить если ли в локал сторедж и проверить время годности 
            // опционально отправить на сервер , для верификации  и получиться ответ
            // создать клаимсы , добавить в  локал сторедж
            // изменить  стейт аунтефикации

            await _localStorage.SetItemAsync(KEY, name);
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, name), }, AUTHSCHEME);

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }


        public async Task LogOut()
        {
            await _localStorage.RemoveItemAsync(KEY);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
