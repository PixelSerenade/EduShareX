<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="EduX.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card dashboard-profile-card">
        <asp:Literal ID="ProfileLiteral" runat="server" />
    </div>
    <div class="grid grid-2">
        <div class="card dashboard-section-card"><h2>My Uploads</h2><asp:Literal ID="UploadsLiteral" runat="server" /></div>
        <div class="card dashboard-section-card"><h2>My Bookmarks</h2><asp:Literal ID="BookmarksLiteral" runat="server" /></div>
    </div>
    <div class="card dashboard-section-card">
        <h2>My Ratings</h2>
        <asp:Literal ID="RatingsLiteral" runat="server" />
    </div>
    <div class="card dashboard-edit-card" id="edit-profile">
        <h2>Edit Profile</h2>
        <asp:Panel ID="MessagePanel" runat="server" Visible="false" CssClass="message success">
            <asp:Literal ID="MessageLiteral" runat="server" />
        </asp:Panel>
        <div class="form-grid">
            <div><label>Full Name</label><asp:TextBox ID="FullNameTextBox" runat="server" /></div>
            <div><label>Department</label><asp:DropDownList ID="DepartmentDropDown" runat="server" /></div>
            <div><label>Update Profile Photo</label><asp:FileUpload ID="ProfilePhotoUpload" runat="server" /></div>
            <div id="DashboardExpertiseWrap" runat="server">
                <label>Subject Expertise</label>
                <div class="expertise-grid">
                    <asp:DropDownList ID="SubjectExpertiseDropDown1" runat="server" CssClass="auth-input" />
                    <asp:DropDownList ID="SubjectExpertiseDropDown2" runat="server" CssClass="auth-input" />
                </div>
            </div>
        </div>
        <div class="table-actions">
            <asp:Button ID="SaveProfileButton" runat="server" Text="Save Profile" OnClick="SaveProfileButton_Click" />
        </div>
    </div>
</asp:Content>
