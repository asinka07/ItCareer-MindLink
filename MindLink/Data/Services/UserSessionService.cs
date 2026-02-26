using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using MindLink.Data.Models;
using System.Security.Claims;

namespace MindLink.Data.Services
{
    public class UserSessionService
    {

        public User? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        public async Task LogIn(User user)
        {
            CurrentUser = user;
        }

        public async Task LogOut()
        {
            CurrentUser = null;
        }
    }
}
