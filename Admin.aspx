<%@ Page Title="Review Desk" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="EduX.Admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card admin-hero-card">
        <span class="eyebrow">Lecturer Review</span>
        <h1>Lecturer Review Desk</h1>
        <asp:Literal ID="AdminStatsLiteral" runat="server" />
    </div>
    <div class="grid grid-2">
        <div class="card admin-section-card"><h2>Notes Waiting for You</h2><asp:Literal ID="PendingLiteral" runat="server" /></div>
        <div class="card admin-section-card"><h2>Reported Notes in Your Queue</h2><asp:Literal ID="ReportedLiteral" runat="server" /></div>
    </div>
</asp:Content>
