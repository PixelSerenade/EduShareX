<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="EduX.Register" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function edxToggleExpertise() {
            var roles = document.getElementsByName('<%= RoleOptions.UniqueID %>');
            var isLecturer = false;
            for (var i = 0; i < roles.length; i++) {
                if (roles[i].checked && roles[i].value === 'Lecturer') {
                    isLecturer = true;
                    break;
                }
            }

            var wrap = document.getElementById('expertiseWrap');
            if (wrap) {
                wrap.style.display = isLecturer ? 'block' : 'none';
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            edxToggleExpertise();
            var roles = document.getElementsByName('<%= RoleOptions.UniqueID %>');
            for (var i = 0; i < roles.length; i++) {
                roles[i].addEventListener('change', edxToggleExpertise);
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-shell">
        <div class="auth-panel auth-panel-hero auth-panel-hero-register">
            <span class="eyebrow">Join The Network</span>
            <h1>Create a study-sharing identity that fits your role.</h1>
            <p class="muted">Students contribute and learn, alumni strengthen the archive, and lecturers review subject-specific uploads that match their expertise.</p>
            <div class="auth-highlights">
                <div class="auth-highlight">
                    <strong>Student</strong>
                    <span>Upload class notes, bookmark trusted material, and prepare faster.</span>
                </div>
                <div class="auth-highlight">
                    <strong>Lecturer</strong>
                    <span>Review notes for your subjects and keep the platform academically reliable.</span>
                </div>
                <div class="auth-highlight">
                    <strong>Alumni</strong>
                    <span>Give back with polished revision packs and exam-focused resources.</span>
                </div>
            </div>
        </div>

        <div class="auth-panel auth-panel-form">
            <h2>Register</h2>
            <asp:Panel ID="MessagePanel" runat="server" Visible="false" CssClass="message error">
                <asp:Literal ID="MessageLiteral" runat="server" />
            </asp:Panel>
            <div class="auth-form-stack">
                <div>
                    <label>Full Name</label>
                    <asp:TextBox ID="FullNameTextBox" runat="server" CssClass="input-strong" />
                </div>
                <div>
                    <label>Department</label>
                    <asp:DropDownList ID="DepartmentDropDown" runat="server" CssClass="input-strong auth-input" />
                </div>
                <div class="auth-wide">
                    <label>Choose Role</label>
                    <asp:RadioButtonList ID="RoleOptions" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="role-selector role-selector-inline" />
                </div>
                <div id="expertiseWrap" class="expertise-wrap">
                    <label>Lecturer Subject Expertise</label>
                    <div class="expertise-grid">
                        <asp:DropDownList ID="SubjectExpertiseDropDown1" runat="server" CssClass="input-strong auth-input" />
                        <asp:DropDownList ID="SubjectExpertiseDropDown2" runat="server" CssClass="input-strong auth-input" />
                    </div>
                    <p class="muted helper-text">Only lecturers need this. Select up to 2 subjects for review assignment.</p>
                </div>
                <div>
                    <label>Email</label>
                    <asp:TextBox ID="EmailTextBox" runat="server" TextMode="Email" CssClass="input-strong auth-input" />
                </div>
                <div>
                    <label>Profile Photo</label>
                    <asp:FileUpload ID="ProfilePhotoUpload" runat="server" CssClass="auth-input" />
                    <p class="muted helper-text">Optional. Upload a square photo for your profile menu.</p>
                </div>
                <div>
                    <label>Password</label>
                    <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="input-strong auth-input" />
                    <p class="muted helper-text">Use 6 to 12 characters with uppercase, lowercase, and one symbol.</p>
                </div>
                <div>
                    <label>Confirm Password</label>
                    <asp:TextBox ID="ConfirmPasswordTextBox" runat="server" TextMode="Password" CssClass="input-strong auth-input" />
                </div>
            </div>
            <div class="table-actions auth-actions">
                <asp:Button ID="RegisterButton" runat="server" Text="Create My Account" OnClick="RegisterButton_Click" CssClass="btn auth-submit" />
                <a class="btn btn-light" href="Login.aspx">Back to Login</a>
            </div>
        </div>
    </div>
</asp:Content>
