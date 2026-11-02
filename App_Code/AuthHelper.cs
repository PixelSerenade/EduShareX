using System.Web;

namespace EduX
{
    public static class AuthHelper
    {
        public static PlatformUser CurrentUser
        {
            get
            {
                var email = HttpContext.Current.Session["CurrentUserEmail"] as string;
                return string.IsNullOrWhiteSpace(email) ? null : EduShareRepository.GetUserByEmail(email);
            }
        }

        public static bool IsLoggedIn
        {
            get { return CurrentUser != null; }
        }

        public static bool IsLecturerModerator
        {
            get { return CurrentUser != null && CurrentUser.Role == UserRole.Lecturer; }
        }

        public static void SignIn(PlatformUser user)
        {
            HttpContext.Current.Session["CurrentUserEmail"] = user.Email;
        }

        public static void SignOut()
        {
            HttpContext.Current.Session.Remove("CurrentUserEmail");
        }
    }
}
