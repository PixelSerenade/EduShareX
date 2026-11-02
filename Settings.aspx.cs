using System;

namespace EduX
{
    public partial class Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthHelper.IsLoggedIn)
            {
                Response.Redirect("~/Login.aspx?returnUrl=Settings.aspx");
                return;
            }
        }

        protected void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            MessagePanel.Visible = true;
            MessageLiteral.Text = "Your settings were saved.";
        }
    }
}
