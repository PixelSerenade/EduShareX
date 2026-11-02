using System;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace EduX
{
    public partial class Notes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSubjects();
                BindNotes();
            }
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            BindNotes();
        }

        private void BindNotes()
        {
            var notes = EduShareRepository.SearchNotes(SubjectDropDown.SelectedValue.Trim(), KeywordTextBox.Text.Trim()).ToList();
            if (notes.Count == 0)
            {
                NotesLiteral.Text = "<p class='muted'>No notes matched your search.</p>";
                return;
            }

            var builder = new StringBuilder();
            foreach (var note in notes)
            {
                builder.Append("<div class='note-row note-card'>");
                builder.Append("<div class='note-card-top'>");
                builder.Append("<div>");
                builder.Append("<h3>" + Server.HtmlEncode(note.Title) + "</h3>");
                builder.Append("<p class='muted note-meta-line'>" + Server.HtmlEncode(note.Subject) + " | Semester " + note.Semester + " | " + Server.HtmlEncode(note.Course) + "</p>");
                builder.Append("</div>");
                builder.Append("<span class='note-quality-badge'>Quality " + note.SmartQualityScore + "</span>");
                builder.Append("</div>");
                builder.Append("<div class='note-pill-row'>");
                builder.Append("<span class='pill'>Rating " + note.AverageRating + "</span>");
                builder.Append("<span class='pill'>Downloads " + note.DownloadCount + "</span>");
                builder.Append("</div>");
                builder.Append("<a class='btn' href='NoteDetails.aspx?id=" + note.Id + "'>Open Details</a>");
                builder.Append("</div>");
            }

            NotesLiteral.Text = builder.ToString();
        }

        private void BindSubjects()
        {
            SubjectDropDown.Items.Clear();
            SubjectDropDown.Items.Add(new ListItem("All Subjects", string.Empty));
            foreach (var subject in EduShareRepository.GetAvailableSubjects())
            {
                SubjectDropDown.Items.Add(new ListItem(subject, subject));
            }
        }
    }
}
