using MindLink.Data.Models;

namespace MindLink.Data.Services
{
    public class UserSessionService
    {
        public User? CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        public void LogIn(User user)
        {
            CurrentUser = user;
        }

        public void LogOut()
        {
            CurrentUser = null;
        }
    }
}
