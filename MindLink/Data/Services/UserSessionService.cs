using MindLink.Data.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace MindLink.Data.Services
{
    public class UserSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public User? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        public async Task LogIn(User user)
        {
            CurrentUser = user;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserCode)
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext!
                .SignInAsync("MyCookieAuth", principal);
        }

        public async Task LogOut()
        {
            CurrentUser = null;
            await _httpContextAccessor.HttpContext!
                .SignOutAsync("MyCookieAuth");
        }
    }
}
