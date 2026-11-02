<%@ Page Title="Analytics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Analytics.aspx.cs" Inherits="EduX.Analytics" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card analytics-hero-card">
        <span class="eyebrow">Platform Insights</span>
        <h1>Analytics</h1>
        <asp:Literal ID="OverviewLiteral" runat="server" />
    </div>
    <div class="grid grid-2">
        <div class="card analytics-section-card"><h2>Top Quality Notes</h2><asp:Literal ID="TopNotesLiteral" runat="server" /></div>
        <div class="card analytics-section-card"><h2>Top Contributors</h2><asp:Literal ID="TopContributorsLiteral" runat="server" /></div>
    </div>
</asp:Content>
