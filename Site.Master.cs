using System;

namespace EduX
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = AuthHelper.CurrentUser;
            var isLoggedIn = user != null;

            if (UserPanel != null)
            {
                UserPanel.Visible = isLoggedIn;
            }

            if (GuestPanel != null)
            {
                GuestPanel.Visible = !isLoggedIn;
            }

            if (UploadLink != null)
            {
                UploadLink.HRef = ResolveClientUrl(isLoggedIn ? "~/Upload.aspx" : "~/Login.aspx?returnUrl=Upload.aspx&reason=loginrequired");
            }

            if (DashboardLink != null)
            {
                DashboardLink.HRef = ResolveClientUrl(isLoggedIn ? "~/Dashboard.aspx" : "~/Login.aspx?returnUrl=Dashboard.aspx&reason=loginrequired");
            }

            var canReview = isLoggedIn && user.Role == UserRole.Lecturer;
            if (ReviewLink != null)
            {
                ReviewLink.Visible = canReview;
                ReviewLink.HRef = ResolveClientUrl(canReview ? "~/Admin.aspx" : "~/Login.aspx");
            }

            if (ReviewMenuLink != null)
            {
                ReviewMenuLink.Visible = canReview;
            }

            if (isLoggedIn)
            {
                if (UserLabel != null)
                {
                    UserLabel.Text = user.FullName + " (" + user.Role + ")";
                }

                if (ProfilePhotoImage != null)
                {
                    ProfilePhotoImage.ImageUrl = string.IsNullOrWhiteSpace(user.ProfilePhotoPath)
                        ? ResolveClientUrl("~/Content/default-avatar.svg")
                        : ResolveClientUrl(user.ProfilePhotoPath);
                }
            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            AuthHelper.SignOut();
            Response.Redirect("~/Default.aspx");
        }
    }
}
