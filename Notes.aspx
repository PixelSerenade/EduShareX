<%@ Page Title="Notes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Notes.aspx.cs" Inherits="EduX.Notes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card notes-filter-card">
        <span class="eyebrow">Explore Library</span>
        <h1>Browse Notes</h1>
        <p class="muted">Search by subject or keyword to find revision-ready notes, lecturer-reviewed material, and focused study packs.</p>
        <div class="form-grid">
            <div>
                <label>Subject</label>
                <asp:DropDownList ID="SubjectDropDown" runat="server" />
            </div>
            <div>
                <label>Keyword</label>
                <asp:TextBox ID="KeywordTextBox" runat="server" />
            </div>
        </div>
        <div class="table-actions">
            <asp:Button ID="SearchButton" runat="server" Text="Search" OnClick="SearchButton_Click" />
        </div>
    </div>
    <div class="card notes-results-card">
        <asp:Literal ID="NotesLiteral" runat="server" />
    </div>
</asp:Content>
