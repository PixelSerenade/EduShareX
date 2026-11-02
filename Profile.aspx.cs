using System;
using System.IO;
using System.Linq;
using System.Text;

namespace EduX
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthHelper.IsLoggedIn)
            {
                Response.Redirect("~/Login.aspx?returnUrl=Profile.aspx&reason=loginrequired");
                return;
            }

            if (!IsPostBack)
            {
                BindProfile();
            }
        }

        protected void SavePhotoButton_Click(object sender, EventArgs e)
        {
            if (!ProfilePhotoUpload.HasFile)
            {
                ShowMessage("Please choose a profile photo first.", true);
                return;
            }

            var extension = Path.GetExtension(ProfilePhotoUpload.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains((extension ?? string.Empty).ToLowerInvariant()))
            {
                ShowMessage("Please upload a JPG, PNG, or WEBP image.", true);
                return;
            }

            var folder = Server.MapPath("~/Content/ProfilePhotos");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileName = "profile_" + Guid.NewGuid().ToString("N") + extension.ToLowerInvariant();
            ProfilePhotoUpload.SaveAs(Path.Combine(folder, fileName));
            AuthHelper.CurrentUser.ProfilePhotoPath = "~/Content/ProfilePhotos/" + fileName;
            ShowMessage("Profile photo updated successfully.", false);
            BindProfile();
        }

        private void BindProfile()
        {
            var user = AuthHelper.CurrentUser;
            var expertise = string.IsNullOrWhiteSpace(user.SubjectExpertise) ? "Not set yet" : user.SubjectExpertise;

            ProfileImage.ImageUrl = string.IsNullOrWhiteSpace(user.ProfilePhotoPath)
                ? ResolveClientUrl("~/Content/default-avatar.svg")
                : ResolveClientUrl(user.ProfilePhotoPath);

            var headerBuilder = new StringBuilder();
            headerBuilder.Append("<span class='eyebrow'>Registered Details</span>");
            headerBuilder.Append("<h1>" + Server.HtmlEncode(user.FullName) + "</h1>");
            headerBuilder.Append("<p class='muted profile-subtitle'>" + Server.HtmlEncode(user.Email) + "</p>");
            headerBuilder.Append("<span class='note-quality-badge'>" + user.Role + "</span>");
            ProfileHeaderLiteral.Text = headerBuilder.ToString();

            var detailsBuilder = new StringBuilder();
            detailsBuilder.Append("<div class='detail-fact'><strong>Full Name</strong><span>" + Server.HtmlEncode(user.FullName) + "</span></div>");
            detailsBuilder.Append("<div class='detail-fact'><strong>Email</strong><span>" + Server.HtmlEncode(user.Email) + "</span></div>");
            detailsBuilder.Append("<div class='detail-fact'><strong>Department</strong><span>" + Server.HtmlEncode(user.Department) + "</span></div>");
            detailsBuilder.Append("<div class='detail-fact'><strong>Role</strong><span>" + user.Role + "</span></div>");
            detailsBuilder.Append("<div class='detail-fact'><strong>Subject Expertise</strong><span>" + Server.HtmlEncode(expertise) + "</span></div>");
            detailsBuilder.Append("<div class='detail-fact'><strong>Reputation</strong><span>" + user.ReputationPoints + "</span></div>");
            ProfileDetailsLiteral.Text = detailsBuilder.ToString();
        }

        private void ShowMessage(string message, bool isError)
        {
            MessagePanel.Visible = true;
            MessagePanel.CssClass = "message " + (isError ? "error" : "success");
            MessageLiteral.Text = message;
        }
    }
}
