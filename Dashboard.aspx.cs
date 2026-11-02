using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EduX
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthHelper.IsLoggedIn)
            {
                Response.Redirect("~/Login.aspx?returnUrl=Dashboard.aspx&reason=loginrequired");
                return;
            }

            if (!IsPostBack)
            {
                FullNameTextBox.Text = AuthHelper.CurrentUser.FullName;
                DepartmentDropDown.DataSource = EduShareRepository.GetAvailableDepartments();
                DepartmentDropDown.DataBind();
                if (DepartmentDropDown.Items.FindByValue(AuthHelper.CurrentUser.Department) != null)
                {
                    DepartmentDropDown.SelectedValue = AuthHelper.CurrentUser.Department;
                }

                BindSubjectDropDown(SubjectExpertiseDropDown1);
                BindSubjectDropDown(SubjectExpertiseDropDown2);
                BindSubjectSelections(AuthHelper.CurrentUser.SubjectExpertise);
                DashboardExpertiseWrap.Visible = AuthHelper.CurrentUser.Role == UserRole.Lecturer;
                BindDashboard();
            }
        }

        protected void SaveProfileButton_Click(object sender, EventArgs e)
        {
            var selectedSubjects = GetSelectedSubjects();

            if (AuthHelper.CurrentUser.Role == UserRole.Lecturer && selectedSubjects.Count == 0)
            {
                MessagePanel.Visible = true;
                MessagePanel.CssClass = "message error";
                MessageLiteral.Text = "Lecturers must select at least one subject.";
                return;
            }

            if (selectedSubjects.Count > 2)
            {
                MessagePanel.Visible = true;
                MessagePanel.CssClass = "message error";
                MessageLiteral.Text = "Lecturers can select up to 2 subjects only.";
                return;
            }

            EduShareRepository.UpdateProfile(AuthHelper.CurrentUser.Email, FullNameTextBox.Text.Trim(), DepartmentDropDown.SelectedValue, string.Join(", ", selectedSubjects));
            SaveProfilePhoto();
            MessagePanel.Visible = true;
            MessagePanel.CssClass = "message success";
            MessageLiteral.Text = "Profile updated successfully.";
            BindDashboard();
        }

        private void BindDashboard()
        {
            var user = AuthHelper.CurrentUser;
            var expertise = string.IsNullOrWhiteSpace(user.SubjectExpertise) ? "Not set yet" : user.SubjectExpertise;
            var profileBuilder = new StringBuilder();
            profileBuilder.Append("<span class='eyebrow'>My Workspace</span>");
            profileBuilder.Append("<div class='dashboard-profile-top'>");
            profileBuilder.Append("<div><h1>" + Server.HtmlEncode(user.FullName) + "</h1><p class='muted'>" + user.Role + " dashboard</p></div>");
            profileBuilder.Append("<span class='note-quality-badge'>Reputation " + user.ReputationPoints + "</span>");
            profileBuilder.Append("</div>");
            profileBuilder.Append("<div class='detail-facts'>");
            profileBuilder.Append("<div class='detail-fact'><strong>Department</strong><span>" + Server.HtmlEncode(user.Department) + "</span></div>");
            profileBuilder.Append("<div class='detail-fact'><strong>Role</strong><span>" + user.Role + "</span></div>");
            profileBuilder.Append("<div class='detail-fact'><strong>Expertise</strong><span>" + Server.HtmlEncode(expertise) + "</span></div>");
            profileBuilder.Append("</div>");
            ProfileLiteral.Text = profileBuilder.ToString();
            UploadsLiteral.Text = RenderNotes(EduShareRepository.GetNotesByContributor(user.Email));
            BookmarksLiteral.Text = RenderNotes(EduShareRepository.GetBookmarkedNotes(user.Email));

            var ratings = EduShareRepository.GetRatingsByUser(user.Email).ToList();
            if (ratings.Count == 0)
            {
                RatingsLiteral.Text = "<p class='muted'>You have not rated any notes yet.</p>";
            }
            else
            {
                var builder = new StringBuilder();
                foreach (var item in ratings)
                {
                    builder.Append("<div class='metric-row dashboard-item-card'><strong>" + Server.HtmlEncode(item.Item1.Title) + "</strong><p class='muted'>Your rating: " + item.Item2.Score + "/5 | Average: " + item.Item1.AverageRating + "</p></div>");
                }
                RatingsLiteral.Text = builder.ToString();
            }
        }

        private string RenderNotes(System.Collections.Generic.IEnumerable<AcademicNote> notes)
        {
            var list = notes.ToList();
            if (list.Count == 0)
            {
                return "<p class='muted'>No items yet.</p>";
            }

            var builder = new StringBuilder();
            foreach (var note in list)
            {
                builder.Append("<div class='metric-row dashboard-item-card'><strong>" + Server.HtmlEncode(note.Title) + "</strong><p class='muted'>" + Server.HtmlEncode(note.Subject) + " | Quality " + note.SmartQualityScore + "</p></div>");
            }
            return builder.ToString();
        }

        private void BindSubjectSelections(string expertise)
        {
            var selectedValues = (expertise ?? string.Empty)
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Trim())
                .ToList();
            if (selectedValues.Count > 0 && SubjectExpertiseDropDown1.Items.FindByValue(selectedValues[0]) != null)
            {
                SubjectExpertiseDropDown1.SelectedValue = selectedValues[0];
            }

            if (selectedValues.Count > 1 && SubjectExpertiseDropDown2.Items.FindByValue(selectedValues[1]) != null)
            {
                SubjectExpertiseDropDown2.SelectedValue = selectedValues[1];
            }
        }

        private void BindSubjectDropDown(DropDownList dropDown)
        {
            dropDown.Items.Clear();
            dropDown.Items.Add(new ListItem("Select Subject", string.Empty));
            foreach (var subject in EduShareRepository.GetAvailableSubjects())
            {
                dropDown.Items.Add(new ListItem(subject, subject));
            }
        }

        private System.Collections.Generic.List<string> GetSelectedSubjects()
        {
            return new[] { SubjectExpertiseDropDown1.SelectedValue, SubjectExpertiseDropDown2.SelectedValue }
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private void SaveProfilePhoto()
        {
            if (!ProfilePhotoUpload.HasFile)
            {
                return;
            }

            var extension = Path.GetExtension(ProfilePhotoUpload.FileName);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            if (!allowedExtensions.Contains((extension ?? string.Empty).ToLowerInvariant()))
            {
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
        }
    }
}
