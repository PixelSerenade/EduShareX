using System;
using System.Linq;
using System.Text;

namespace EduX
{
    public partial class Upload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthHelper.IsLoggedIn)
            {
                Response.Redirect("~/Login.aspx?returnUrl=Upload.aspx&reason=loginrequired");
                return;
            }

            if (!IsPostBack)
            {
                DifficultyDropDown.DataSource = new[] { "Foundation", "Intermediate", "Advanced" };
                DifficultyDropDown.DataBind();
                SubjectDropDown.DataSource = EduShareRepository.GetAvailableSubjects();
                SubjectDropDown.DataBind();
                CourseDropDown.DataSource = EduShareRepository.GetAvailableCourses();
                CourseDropDown.DataBind();
                SemesterDropDown.DataSource = Enumerable.Range(1, 6).Select(value => value.ToString()).ToList();
                SemesterDropDown.DataBind();
                ContentTypeDropDown.DataSource = new[] { "Typed PDF", "Handwritten Notes", "PPT Slides", "DOC Notes" };
                ContentTypeDropDown.DataBind();
                TagSuggestionLiteral.Text = BuildTagSuggestions();
            }
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (!NoteFileUpload.HasFile)
            {
                ShowMessage("Please select a file to upload.", true);
                return;
            }

            int semester;
            int readingMinutes;
            int conceptCoverage;
            if (!int.TryParse(SemesterDropDown.SelectedValue, out semester))
            {
                ShowMessage("Please choose a valid semester.", true);
                return;
            }

            if (!int.TryParse(ReadingMinutesTextBox.Text, out readingMinutes) || readingMinutes <= 0)
            {
                ShowMessage("Reading minutes must be greater than zero.", true);
                return;
            }

            if (!int.TryParse(CoverageTextBox.Text, out conceptCoverage) || conceptCoverage <= 0)
            {
                ShowMessage("Concept coverage must be a positive number.", true);
                return;
            }

            var note = EduShareRepository.AddNote(
                AuthHelper.CurrentUser,
                TitleTextBox.Text.Trim(),
                SubjectDropDown.SelectedValue,
                semester,
                CourseDropDown.SelectedValue,
                DifficultyDropDown.SelectedValue,
                ContentTypeDropDown.SelectedValue,
                readingMinutes,
                conceptCoverage,
                TagsTextBox.Text.Trim(),
                NoteFileUpload.PostedFile);

            var assignedLecturer = string.IsNullOrWhiteSpace(note.AssignedLecturerName) ? "the lecturer review queue" : note.AssignedLecturerName;
            ShowMessage("Your note has been submitted and is now waiting for review from " + assignedLecturer + ".", false);
        }

        private void ShowMessage(string message, bool isError)
        {
            MessagePanel.Visible = true;
            MessagePanel.CssClass = "message " + (isError ? "error" : "success");
            MessageLiteral.Text = message;
        }

        private string BuildTagSuggestions()
        {
            var builder = new StringBuilder();
            foreach (var tag in EduShareRepository.GetAvailableTags())
            {
                builder.Append("<option value='");
                builder.Append(Server.HtmlEncode(tag));
                builder.Append("'></option>");
            }

            return builder.ToString();
        }
    }
}
