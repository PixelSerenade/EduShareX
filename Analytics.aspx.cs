using System;
using System.Linq;
using System.Text;

namespace EduX
{
    public partial class Analytics : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var stats = EduShareRepository.GetStats();
            var builder = new StringBuilder();
            builder.Append("<div class='detail-facts'>");
            builder.Append("<div class='detail-fact'><strong>Total Downloads</strong><span>" + stats.TotalDownloads + "</span></div>");
            builder.Append("<div class='detail-fact'><strong>Approved Notes</strong><span>" + stats.ApprovedNotes + "</span></div>");
            builder.Append("<div class='detail-fact'><strong>Reports</strong><span>" + stats.TotalReports + "</span></div>");
            builder.Append("</div>");
            OverviewLiteral.Text = builder.ToString();
            TopNotesLiteral.Text = RenderTopNotes();
            TopContributorsLiteral.Text = RenderTopContributors();
        }

        private string RenderTopNotes()
        {
            var builder = new StringBuilder();
            foreach (var note in EduShareRepository.GetNotes().Where(item => item.IsApproved).OrderByDescending(item => item.SmartQualityScore).Take(5))
            {
                builder.Append("<div class='metric-row admin-item-card'><strong>" + Server.HtmlEncode(note.Title) + "</strong><p class='muted'>Quality " + note.SmartQualityScore + " | Downloads " + note.DownloadCount + "</p></div>");
            }
            return builder.ToString();
        }

        private string RenderTopContributors()
        {
            var builder = new StringBuilder();
            foreach (var user in EduShareRepository.GetUsers().OrderByDescending(item => item.ReputationPoints).Take(5))
            {
                builder.Append("<div class='metric-row admin-item-card'><strong>" + Server.HtmlEncode(user.FullName) + "</strong><p class='muted'>" + user.Role + " | Reputation " + user.ReputationPoints + "</p></div>");
            }
            return builder.ToString();
        }
    }
}
