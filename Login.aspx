<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EduX.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-shell">
        <div class="auth-panel auth-panel-hero">
            <asp:Literal ID="HeroEyebrowLiteral" runat="server" />
            <asp:Literal ID="HeroTitleLiteral" runat="server" />
            <asp:Literal ID="HeroDescriptionLiteral" runat="server" />
            <div class="auth-badges">
                <span class="auth-badge">Smart note discovery</span>
                <span class="auth-badge">Lecturer-reviewed quality</span>
                <span class="auth-badge">Bookmarks and ratings</span>
            </div>
        </div>

        <div class="auth-panel auth-panel-form">
            <h2>Login</h2>
            <asp:Panel ID="MessagePanel" runat="server" Visible="false" CssClass="message error">
                <asp:Literal ID="MessageLiteral" runat="server" />
            </asp:Panel>
            <div class="auth-form-stack">
                <div class="auth-field">
                    <label>Email</label>
                    <asp:TextBox ID="EmailTextBox" runat="server" TextMode="Email" CssClass="input-strong auth-input" />
                </div>
                <div class="auth-field">
                    <label>Password</label>
                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="input-strong auth-input" />
                </div>
            </div>
            <div class="table-actions auth-actions">
                <asp:Button ID="LoginButton" runat="server" Text="Enter EduShare X" OnClick="LoginButton_Click" CssClass="btn auth-submit" />
                <a class="btn btn-light" href="Register.aspx">Create Account</a>
            </div>
            <p class="muted auth-footnote">Students, lecturers, and alumni use the same sign-in page. Lecturer accounts open the review desk automatically from the main navigation.</p>
        </div>
    </div>
</asp:Content>
