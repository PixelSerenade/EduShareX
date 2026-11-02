<%@ Page Title="Settings" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="EduX.Settings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="card">
        <h1>Settings</h1>
        <asp:Panel ID="MessagePanel" runat="server" Visible="false" CssClass="message success">
            <asp:Literal ID="MessageLiteral" runat="server" />
        </asp:Panel>
        <div class="form-grid">
            <div>
                <label>Theme</label>
                <div class="theme-toggle-row settings-theme-row">
                    <span>Dark Theme</span>
                    <label class="theme-switch">
                        <input type="checkbox" class="theme-toggle-input" onchange="edxSetTheme(this.checked ? 'dark' : 'light');" />
                        <span class="theme-slider"></span>
                    </label>
                </div>
                <p class="muted helper-text">By default the app follows your device theme until you toggle it.</p>
            </div>
            <div>
                <label>Notification Preference</label>
                <asp:DropDownList ID="NotificationDropDown" runat="server">
                    <asp:ListItem Text="All updates" Value="All updates" />
                    <asp:ListItem Text="Only review updates" Value="Only review updates" />
                    <asp:ListItem Text="Bookmarks and downloads" Value="Bookmarks and downloads" />
                </asp:DropDownList>
            </div>
            <div>
                <label>Profile Visibility</label>
                <asp:DropDownList ID="VisibilityDropDown" runat="server">
                    <asp:ListItem Text="Campus only" Value="Campus only" />
                    <asp:ListItem Text="Visible to all users" Value="Visible to all users" />
                    <asp:ListItem Text="Only lecturers" Value="Only lecturers" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="table-actions">
            <asp:Button ID="SaveSettingsButton" runat="server" Text="Save Settings" OnClick="SaveSettingsButton_Click" />
        </div>
    </div>
</asp:Content>
