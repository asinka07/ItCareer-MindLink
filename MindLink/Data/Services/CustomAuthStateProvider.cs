using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using MindLink.Data.Services;

namespace MindLink.Data.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly UserSessionService _session;

        public CustomAuthStateProvider(UserSessionService session)
        {
            _session = session;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity;

            if (_session.IsLoggedIn && _session.CurrentUser != null)
            {
                identity = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, _session.CurrentUser.Username),
            new Claim(ClaimTypes.Role, _session.CurrentUser.Role?.Name ?? ""),
            new Claim("UserCode", _session.CurrentUser.UserCode)
        }, "CustomAuth");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }

        public void NotifyUserChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
