using System;
using System.Linq;
using System.Text;

namespace EduX
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                return;
            }

            var stats = EduShareRepository.GetStats();
            ShareNotesLink.NavigateUrl = AuthHelper.IsLoggedIn ? "~/Upload.aspx" : "~/Login.aspx?returnUrl=Upload.aspx&reason=loginrequired";
            DashboardHeroLink.NavigateUrl = AuthHelper.IsLoggedIn ? "~/Dashboard.aspx" : "~/Login.aspx?returnUrl=Dashboard.aspx&reason=loginrequired";
            TotalUsersLiteral.Text = stats.TotalUsers.ToString();
            TotalNotesLiteral.Text = stats.TotalNotes.ToString();
            PendingNotesLiteral.Text = stats.PendingNotes.ToString();
            ApprovedNotesLiteral.Text = stats.ApprovedNotes.ToString();
            DownloadsLiteral.Text = stats.TotalDownloads.ToString();
            ReportsLiteral.Text = stats.TotalReports.ToString();
            BindHighlights();
        }

        private void BindHighlights()
        {
            var featuredNotes = EduShareRepository.GetNotes()
                .Where(item => item.IsApproved)
                .OrderByDescending(item => item.SmartQualityScore)
                .Take(3)
                .ToList();

            var featuredBuilder = new StringBuilder();
            foreach (var note in featuredNotes)
            {
                featuredBuilder.Append("<div class='note-row'>");
                featuredBuilder.Append("<strong>" + Server.HtmlEncode(note.Title) + "</strong>");
                featuredBuilder.Append("<p class='muted'>" + Server.HtmlEncode(note.Subject) + " | Rating " + note.AverageRating + " | Quality " + note.SmartQualityScore + "</p>");
                featuredBuilder.Append("<a class='btn btn-secondary' href='NoteDetails.aspx?id=" + note.Id + "'>View</a>");
                featuredBuilder.Append("</div>");
            }
            FeaturedNotesLiteral.Text = featuredBuilder.ToString();

            var topContributors = EduShareRepository.GetUsers()
                .OrderByDescending(item => item.ReputationPoints)
                .Take(3)
                .ToList();

            var highlightBuilder = new StringBuilder();
            foreach (var contributor in topContributors)
            {
                highlightBuilder.Append("<div class='metric-row'>");
                highlightBuilder.Append("<strong>" + Server.HtmlEncode(contributor.FullName) + "</strong>");
                highlightBuilder.Append("<p class='muted'>" + contributor.Role + " | " + Server.HtmlEncode(contributor.Department) + " | Reputation " + contributor.ReputationPoints + "</p>");
                highlightBuilder.Append("</div>");
            }

            highlightBuilder.Append("<div class='metric-row'>");
            highlightBuilder.Append("<strong>Platform workflow</strong>");
            highlightBuilder.Append("<p class='muted'>Upload -> Review -> Approve -> Discover -> Engage</p>");
            highlightBuilder.Append("</div>");
            HighlightsLiteral.Text = highlightBuilder.ToString();
        }
    }
}
