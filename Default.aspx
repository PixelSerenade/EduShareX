<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EduX._Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="hero-stage">
        <div class="card hero hero-split hero-home">
            <div class="hero-copy">
                <span class="eyebrow">Academic collaboration platform</span>
                <h1>EduShare X keeps campus notes organized, reusable, and quality-checked.</h1>
                <p class="muted">Built with actual ASP.NET Master Pages, this platform gives students, alumni, and lecturers one shared academic workspace for note discovery, contribution, lecturer review, and improvement.</p>
                <div class="table-actions">
                    <a class="btn" href="Notes.aspx">Browse Notes</a>
                    <asp:HyperLink ID="ShareNotesLink" runat="server" CssClass="btn btn-secondary">Share Notes</asp:HyperLink>
                    <asp:HyperLink ID="DashboardHeroLink" runat="server" CssClass="btn btn-light">Open Dashboard</asp:HyperLink>
                </div>
            </div>
            <div class="hero-panel hero-panel-home">
                <h3>What makes it smart?</h3>
                <div class="hero-feature">Metadata-rich uploads with subject, semester, difficulty, and tags</div>
                <div class="hero-feature">Lecturer review before public visibility</div>
                <div class="hero-feature">Ratings, bookmarks, comments, and reports</div>
                <div class="hero-feature">Contributor reputation and quality signals</div>
            </div>
        </div>

        <div class="home-ribbon">
            <div class="home-ribbon-item">
                <strong>Discover</strong>
                <span>Find semester-ready notes in one place.</span>
            </div>
            <div class="home-ribbon-item">
                <strong>Submit</strong>
                <span>Contribute with guided tags and structured metadata.</span>
            </div>
            <div class="home-ribbon-item">
                <strong>Review</strong>
                <span>Lecturers moderate only the subjects they own.</span>
            </div>
        </div>
    </div>
    <div class="grid grid-3 home-stats-grid">
        <div class="stat"><span>Total Users</span><strong><asp:Literal ID="TotalUsersLiteral" runat="server" /></strong></div>
        <div class="stat"><span>Total Notes</span><strong><asp:Literal ID="TotalNotesLiteral" runat="server" /></strong></div>
        <div class="stat"><span>Pending Review</span><strong><asp:Literal ID="PendingNotesLiteral" runat="server" /></strong></div>
        <div class="stat"><span>Approved Notes</span><strong><asp:Literal ID="ApprovedNotesLiteral" runat="server" /></strong></div>
        <div class="stat"><span>Total Downloads</span><strong><asp:Literal ID="DownloadsLiteral" runat="server" /></strong></div>
        <div class="stat"><span>Total Reports</span><strong><asp:Literal ID="ReportsLiteral" runat="server" /></strong></div>
    </div>
    <div class="grid grid-2 home-content-grid">
        <div class="card section-card">
            <h2>Featured Notes</h2>
            <asp:Literal ID="FeaturedNotesLiteral" runat="server" />
        </div>
        <div class="card section-card">
            <h2>Community Highlights</h2>
            <asp:Literal ID="HighlightsLiteral" runat="server" />
        </div>
    </div>
</asp:Content>
