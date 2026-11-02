using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EduX
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RoleOptions.DataSource = new[] { UserRole.Student, UserRole.Lecturer, UserRole.Alumni };
                RoleOptions.DataBind();
                DepartmentDropDown.DataSource = EduShareRepository.GetAvailableDepartments();
                DepartmentDropDown.DataBind();
                BindSubjectDropDown(SubjectExpertiseDropDown1);
                BindSubjectDropDown(SubjectExpertiseDropDown2);
                RoleOptions.SelectedValue = UserRole.Student.ToString();
            }
        }

        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox.Text != ConfirmPasswordTextBox.Text)
            {
                ShowError("Passwords do not match.");
                return;
            }

            var role = (UserRole)Enum.Parse(typeof(UserRole), RoleOptions.SelectedValue);
            var selectedSubjects = GetSelectedSubjects();

            if (!IsPasswordValid(PasswordTextBox.Text))
            {
                ShowError("Password must be 6 to 12 characters long and include uppercase, lowercase, and one symbol.");
                return;
            }

            if (role == UserRole.Lecturer && selectedSubjects.Count == 0)
            {
                ShowError("Lecturers must select at least one subject.");
                return;
            }

            if (selectedSubjects.Count > 2)
            {
                ShowError("Lecturers can select up to 2 subjects only.");
                return;
            }

            var user = new PlatformUser
            {
                FullName = FullNameTextBox.Text.Trim(),
                Department = DepartmentDropDown.SelectedValue,
                SubjectExpertise = string.Join(", ", selectedSubjects),
                Email = EmailTextBox.Text.Trim(),
                ProfilePhotoPath = SaveProfilePhoto(),
                Password = PasswordTextBox.Text,
                Role = role,
                ReputationPoints = 0
            };

            string errorMessage;
            if (!EduShareRepository.RegisterUser(user, out errorMessage))
            {
                ShowError(errorMessage);
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

        private void ShowError(string message)
        {
            MessagePanel.Visible = true;
            MessageLiteral.Text = message;
        }

        private static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[^A-Za-z0-9]).{6,12}$");
        }

        private void BindSubjectDropDown(System.Web.UI.WebControls.DropDownList dropDown)
        {
            dropDown.Items.Clear();
            dropDown.Items.Add(new System.Web.UI.WebControls.ListItem("Select Subject", string.Empty));
            foreach (var subject in EduShareRepository.GetAvailableSubjects())
            {
                dropDown.Items.Add(new System.Web.UI.WebControls.ListItem(subject, subject));
            }
        }

        private System.Collections.Generic.List<string> GetSelectedSubjects()
        {
            return new[] { SubjectExpertiseDropDown1.SelectedValue, SubjectExpertiseDropDown2.SelectedValue }
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private string SaveProfilePhoto()
        {
            if (!ProfilePhotoUpload.HasFile)
            {
                return string.Empty;
            }

            var extension = Path.GetExtension(ProfilePhotoUpload.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains((extension ?? string.Empty).ToLowerInvariant()))
            {
                return string.Empty;
            }

            var folder = Server.MapPath("~/Content/ProfilePhotos");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var fileName = "profile_" + Guid.NewGuid().ToString("N") + extension.ToLowerInvariant();
            ProfilePhotoUpload.SaveAs(Path.Combine(folder, fileName));
            return "~/Content/ProfilePhotos/" + fileName;
        }
    }
}
