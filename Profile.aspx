<%@ Page Title="Profile Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="EduX.Profile" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function edxOpenProfilePhotoPicker() {
            var picker = document.getElementById('<%= ProfilePhotoUpload.ClientID %>');
            if (picker) {
                picker.click();
            }
            return false;
        }

        function edxSubmitProfilePhoto() {
            var picker = document.getElementById('<%= ProfilePhotoUpload.ClientID %>');
            var button = document.getElementById('<%= SavePhotoButton.ClientID %>');
            if (picker && picker.value && button) {
                button.click();
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card profile-details-card">
        <asp:Panel ID="MessagePanel" runat="server" Visible="false" CssClass="message success">
            <asp:Literal ID="MessageLiteral" runat="server" />
        </asp:Panel>

        <div class="profile-center">
            <a href="#" class="profile-photo-editor" onclick="return edxOpenProfilePhotoPicker();">
                <asp:Image ID="ProfileImage" runat="server" CssClass="profile-photo-large" />
                <span class="profile-photo-overlay">Change Photo</span>
            </a>
            <asp:FileUpload ID="ProfilePhotoUpload" runat="server" CssClass="profile-photo-input" onchange="edxSubmitProfilePhoto();" />
            <asp:Button ID="SavePhotoButton" runat="server" Text="Save Photo" CssClass="profile-photo-submit" OnClick="SavePhotoButton_Click" />
            <asp:Literal ID="ProfileHeaderLiteral" runat="server" />
        </div>

        <div class="detail-facts profile-detail-grid">
            <asp:Literal ID="ProfileDetailsLiteral" runat="server" />
        </div>
    </div>

    <div class="card profile-actions-card">
        <h2>Extra Details</h2>
        <div class="table-actions">
            <a class="btn icon-btn" href="Dashboard.aspx#edit-profile">
                <span class="icon-chip">&#9998;</span>
                <span>Edit Details</span>
            </a>
        </div>
    </div>
</asp:Content>
