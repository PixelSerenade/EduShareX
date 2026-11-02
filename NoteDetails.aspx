<%@ Page Title="Note Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NoteDetails.aspx.cs" Inherits="EduX.NoteDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card note-detail-card">
        <asp:Panel ID="MessagePanel" runat="server" Visible="false" CssClass="message success">
            <asp:Literal ID="MessageLiteral" runat="server" />
        </asp:Panel>
        <asp:Literal ID="DetailLiteral" runat="server" />
    </div>
    <div class="card note-actions-card">
        <h2>Actions</h2>
        <div class="table-actions">
            <asp:Button ID="BookmarkButton" runat="server" Text="Toggle Bookmark" OnClick="BookmarkButton_Click" />
            <asp:DropDownList ID="RatingDropDown" runat="server" />
            <asp:Button ID="RateButton" runat="server" Text="Save Rating" OnClick="RateButton_Click" />
        </div>
        <div class="form-grid">
            <div>
                <label>Add Comment</label>
                <asp:TextBox ID="CommentTextBox" runat="server" TextMode="MultiLine" />
                <div class="table-actions">
                    <asp:Button ID="CommentButton" runat="server" Text="Post Comment" OnClick="CommentButton_Click" />
                </div>
            </div>
            <div>
                <label>Report Note</label>
                <asp:TextBox ID="ReportTextBox" runat="server" TextMode="MultiLine" />
                <div class="table-actions">
                    <asp:Button ID="ReportButton" runat="server" Text="Submit Report" CssClass="btn-danger" OnClick="ReportButton_Click" />
                    <asp:Button ID="DownloadButton" runat="server" Text="Download File" CssClass="btn-secondary" OnClick="DownloadButton_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
