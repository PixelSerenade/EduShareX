using System;
using System.Text;

namespace EduX
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthHelper.IsLecturerModerator)
            {
                Response.Redirect("~/Login.aspx?returnUrl=Admin.aspx");
                return;
            }

            var approveId = Request.QueryString["approve"];
            var rejectId = Request.QueryString["reject"];
            int noteId;
            if (int.TryParse(approveId, out noteId))
            {
                EduShareRepository.ApproveNote(noteId, AuthHelper.CurrentUser.Email);
            }
            if (int.TryParse(rejectId, out noteId))
            {
                EduShareRepository.RejectNote(noteId, AuthHelper.CurrentUser.Email);
            }

            BindAdmin();
        }

        private void BindAdmin()
        {
            var stats = EduShareRepository.GetStats();
            var expertise = string.IsNullOrWhiteSpace(AuthHelper.CurrentUser.SubjectExpertise) ? "Not assigned yet" : AuthHelper.CurrentUser.SubjectExpertise;
            var builder = new StringBuilder();
            builder.Append("<div class='detail-facts'>");
            builder.Append("<div class='detail-fact'><strong>Reviewer</strong><span>" + Server.HtmlEncode(AuthHelper.CurrentUser.FullName) + "</span></div>");
            builder.Append("<div class='detail-fact'><strong>Expertise</strong><span>" + Server.HtmlEncode(expertise) + "</span></div>");
            builder.Append("<div class='detail-fact'><strong>Total Notes</strong><span>" + stats.TotalNotes + "</span></div>");
            builder.Append("<div class='detail-fact'><strong>Total Reports</strong><span>" + stats.TotalReports + "</span></div>");
            builder.Append("</div>");
            AdminStatsLiteral.Text = builder.ToString();
            PendingLiteral.Text = RenderModerationList(EduShareRepository.GetPendingNotes(AuthHelper.CurrentUser.Email), true);
            ReportedLiteral.Text = RenderModerationList(EduShareRepository.GetReportedNotes(AuthHelper.CurrentUser.Email), false);
        }

        private string RenderModerationList(System.Collections.Generic.IEnumerable<AcademicNote> notes, bool showApproveReject)
        {
            var builder = new StringBuilder();
            var any = false;
            foreach (var note in notes)
            {
                any = true;
                builder.Append("<div class='metric-row admin-item-card'><strong>" + Server.HtmlEncode(note.Title) + "</strong><p class='muted'>" + Server.HtmlEncode(note.Subject) + " | By " + Server.HtmlEncode(note.ContributorName) + " | Routed to " + Server.HtmlEncode(note.AssignedLecturerName) + "</p>");
                builder.Append("<div class='table-actions'><a class='btn' href='NoteDetails.aspx?id=" + note.Id + "'>Inspect</a>");
                if (showApproveReject)
                {
                    builder.Append("<a class='btn btn-secondary' href='Admin.aspx?approve=" + note.Id + "'>Approve</a>");
                    builder.Append("<a class='btn btn-danger' href='Admin.aspx?reject=" + note.Id + "'>Reject</a>");
                }
                builder.Append("</div></div>");
            }

            if (!any)
            {
                return "<p class='muted'>No items in this section.</p>";
            }

            return builder.ToString();
        }
    }
}
