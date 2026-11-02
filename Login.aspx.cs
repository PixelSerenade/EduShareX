using System;
using System.Web;

namespace EduX
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            var loginRequired = string.Equals(Request.QueryString["reason"], "loginrequired", StringComparison.OrdinalIgnoreCase);
            if (loginRequired)
            {
                HeroEyebrowLiteral.Text = "<span class='eyebrow'>Login Required</span>";
                HeroTitleLiteral.Text = "<h1>You have not logged in yet.</h1>";
                HeroDescriptionLiteral.Text = "<p class='muted'>Please log in to continue to your dashboard or upload notes.</p>";
                MessagePanel.Visible = true;
                MessagePanel.CssClass = "message error";
                MessageLiteral.Text = "You have not logged in. Please log in to continue.";
            }
            else
            {
                HeroEyebrowLiteral.Text = "<span class='eyebrow'>Welcome Back</span>";
                HeroTitleLiteral.Text = "<h1>Step back into your campus knowledge hub.</h1>";
                HeroDescriptionLiteral.Text = "<p class='muted'>Sign in to upload notes, manage your personal dashboard, bookmark revision sets, and track lecturer-reviewed material from one place.</p>";
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            var user = EduShareRepository.ValidateUser(EmailTextBox.Text.Trim(), PasswordTextBox.Text);
            if (user == null)
            {
                MessagePanel.Visible = true;
                MessageLiteral.Text = "Invalid email or password.";
                return;
            }

            AuthHelper.SignIn(user);
            var returnUrl = Request.QueryString["returnUrl"];
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                Response.Redirect("~/" + returnUrl.TrimStart('~', '/'));
                return;
            }

            Response.Redirect("~/Default.aspx");
        }
    }
}
