using System;
using System.IO;
using System.Linq;
using System.Text;

namespace EduX
{
    public partial class NoteDetails : System.Web.UI.Page
    {
        private AcademicNote CurrentNote
        {
            get
            {
                int noteId;
                return int.TryParse(Request.QueryString["id"], out noteId) ? EduShareRepository.GetNote(noteId) : null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RatingDropDown.DataSource = new[] { "1", "2", "3", "4", "5" };
                RatingDropDown.DataBind();
                BindNote();
            }
        }

        protected void BookmarkButton_Click(object sender, EventArgs e)
        {
            if (!RequireLogin())
            {
                return;
            }

            EduShareRepository.ToggleBookmark(CurrentNote.Id, AuthHelper.CurrentUser.Email);
            ShowMessage("Bookmark status updated.");
            BindNote();
        }

        protected void RateButton_Click(object sender, EventArgs e)
        {
            if (!RequireLogin())
            {
                return;
            }

            EduShareRepository.AddOrUpdateRating(CurrentNote.Id, AuthHelper.CurrentUser.Email, int.Parse(RatingDropDown.SelectedValue));
            ShowMessage("Rating saved.");
            BindNote();
        }

        protected void CommentButton_Click(object sender, EventArgs e)
        {
            if (!RequireLogin())
            {
                return;
            }

            EduShareRepository.AddComment(CurrentNote.Id, AuthHelper.CurrentUser.FullName, CommentTextBox.Text.Trim());
            CommentTextBox.Text = string.Empty;
            ShowMessage("Comment posted.");
            BindNote();
        }

        protected void ReportButton_Click(object sender, EventArgs e)
        {
            if (!RequireLogin())
            {
                return;
            }

            EduShareRepository.AddReport(CurrentNote.Id, AuthHelper.CurrentUser.Email, AuthHelper.CurrentUser.FullName, ReportTextBox.Text.Trim());
            ReportTextBox.Text = string.Empty;
            ShowMessage("Report submitted.");
            BindNote();
        }

        protected void DownloadButton_Click(object sender, EventArgs e)
        {
            if (CurrentNote == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentNote.StoredFileName))
            {
                ShowMessage("This sample note does not have an uploaded file yet.");
                return;
            }

            var path = Server.MapPath("~/App_Data/Uploads/" + CurrentNote.StoredFileName);
            if (!File.Exists(path))
            {
                ShowMessage("The file is not available.");
                return;
            }

            EduShareRepository.IncrementDownload(CurrentNote.Id);
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + CurrentNote.FileName);
            Response.TransmitFile(path);
            Response.End();
        }

        private bool RequireLogin()
        {
            if (AuthHelper.IsLoggedIn)
            {
                return true;
            }

            Response.Redirect("~/Login.aspx");
            return false;
        }

        private void BindNote()
        {
            if (CurrentNote == null)
            {
                DetailLiteral.Text = "<h2>Note not found.</h2>";
                return;
            }

            var builder = new StringBuilder();
            builder.Append("<span class='eyebrow'>Note Overview</span>");
            builder.Append("<div class='note-detail-top'>");
            builder.Append("<div>");
            builder.Append("<h1>" + Server.HtmlEncode(CurrentNote.Title) + "</h1>");
            builder.Append("<p class='muted note-meta-line'>" + Server.HtmlEncode(CurrentNote.Subject) + " | Semester " + CurrentNote.Semester + " | " + Server.HtmlEncode(CurrentNote.Course) + "</p>");
            builder.Append("</div>");
            builder.Append("<span class='note-quality-badge'>Quality " + CurrentNote.SmartQualityScore + "</span>");
            builder.Append("</div>");
            builder.Append("<div class='note-pill-row'>");
            builder.Append("<span class='pill'>Rating " + CurrentNote.AverageRating + "</span>");
            builder.Append("<span class='pill'>Downloads " + CurrentNote.DownloadCount + "</span>");
            builder.Append("<span class='pill'>Bookmarks " + CurrentNote.BookmarkEmails.Count + "</span>");
            builder.Append("<span class='pill'>Reports " + CurrentNote.Reports.Count + "</span>");
            builder.Append("</div>");
            builder.Append("<div class='detail-facts'>");
            builder.Append("<div class='detail-fact'><strong>Contributor</strong><span>" + Server.HtmlEncode(CurrentNote.ContributorName) + " (" + CurrentNote.ContributorRole + ")</span></div>");
            builder.Append("<div class='detail-fact'><strong>Reading Time</strong><span>" + CurrentNote.EstimatedReadingMinutes + " mins</span></div>");
            builder.Append("<div class='detail-fact'><strong>Coverage</strong><span>" + CurrentNote.ConceptCoveragePercent + "%</span></div>");
            builder.Append("</div>");
            builder.Append("<div class='note-tag-row'>");
            foreach (var tag in CurrentNote.Tags)
            {
                builder.Append("<span class='pill'>" + Server.HtmlEncode(tag) + "</span>");
            }
            builder.Append("</div><h3>Comments</h3>");
            if (CurrentNote.Comments.Count == 0)
            {
                builder.Append("<p class='muted'>No comments yet.</p>");
            }
            else
            {
                foreach (var comment in CurrentNote.Comments.OrderByDescending(item => item.CreatedOn))
                {
                    builder.Append("<div class='metric-row comment-card'><strong>" + Server.HtmlEncode(comment.AuthorName) + "</strong><p class='muted'>" + Server.HtmlEncode(comment.Message) + "</p></div>");
                }
            }

            DetailLiteral.Text = builder.ToString();
        }

        private void ShowMessage(string message)
        {
            MessagePanel.Visible = true;
            MessageLiteral.Text = message;
        }
    }
}
