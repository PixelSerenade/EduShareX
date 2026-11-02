<%@ Page Title="Upload" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="EduX.Upload" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function edxAddTag() {
            var input = document.getElementById('TagSearchInput');
            var hidden = document.getElementById('<%= TagsTextBox.ClientID %>');
            var bag = document.getElementById('SelectedTags');
            if (!input || !hidden || !bag) {
                return false;
            }

            var value = input.value.trim();
            if (!value) {
                return false;
            }

            var tags = hidden.value ? hidden.value.split(',') : [];
            var exists = false;
            for (var i = 0; i < tags.length; i++) {
                if (tags[i].trim().toLowerCase() === value.toLowerCase()) {
                    exists = true;
                }
            }

            if (!exists) {
                tags.push(value);
            }

            hidden.value = tags.filter(function (t) { return t.trim().length > 0; }).join(', ');
            input.value = '';
            edxRenderTags();
            return false;
        }

        function edxRemoveTag(value) {
            var hidden = document.getElementById('<%= TagsTextBox.ClientID %>');
            var tags = hidden.value ? hidden.value.split(',') : [];
            var filtered = [];
            for (var i = 0; i < tags.length; i++) {
                if (tags[i].trim().toLowerCase() !== value.toLowerCase()) {
                    filtered.push(tags[i].trim());
                }
            }
            hidden.value = filtered.join(', ');
            edxRenderTags();
        }

        function edxRenderTags() {
            var hidden = document.getElementById('<%= TagsTextBox.ClientID %>');
            var bag = document.getElementById('SelectedTags');
            if (!hidden || !bag) {
                return;
            }

            bag.innerHTML = '';
            var tags = hidden.value ? hidden.value.split(',') : [];
            for (var i = 0; i < tags.length; i++) {
                var tag = tags[i].trim();
                if (!tag) {
                    continue;
                }

                var chip = document.createElement('button');
                chip.type = 'button';
                chip.className = 'tag-chip';
                chip.innerHTML = tag + ' <span>x</span>';
                chip.onclick = (function (value) {
                    return function () { edxRemoveTag(value); };
                })(tag);
                bag.appendChild(chip);
            }
        }

        document.addEventListener('DOMContentLoaded', edxRenderTags);
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="upload-shell">
        <div class="card upload-hero">
            <div>
                <span class="eyebrow">Guided Submission</span>
                <h1>Upload notes that are easier to trust, discover, and review.</h1>
                <p class="muted">Use structured metadata, subject tags, and a clear file format so your note reaches the right lecturer and becomes useful faster.</p>
            </div>
            <div class="upload-hero-checklist">
                <div class="upload-tip">Pick the right subject and semester</div>
                <div class="upload-tip">Add relevant tags and coverage details</div>
                <div class="upload-tip">Keep the file readable and exam-ready</div>
            </div>
        </div>

        <div class="card upload-form-card">
            <span class="eyebrow">Submission Form</span>
            <h2>Upload Notes</h2>
            <asp:Panel ID="MessagePanel" runat="server" Visible="false" CssClass="message success">
                <asp:Literal ID="MessageLiteral" runat="server" />
            </asp:Panel>
            <div class="form-grid">
                <div><label>Title</label><asp:TextBox ID="TitleTextBox" runat="server" /></div>
                <div><label>Subject</label><asp:DropDownList ID="SubjectDropDown" runat="server" /></div>
                <div><label>Semester</label><asp:DropDownList ID="SemesterDropDown" runat="server" /></div>
                <div><label>Course</label><asp:DropDownList ID="CourseDropDown" runat="server" /></div>
                <div><label>Difficulty</label><asp:DropDownList ID="DifficultyDropDown" runat="server" /></div>
                <div><label>Content Type</label><asp:DropDownList ID="ContentTypeDropDown" runat="server" /></div>
                <div><label>Reading Minutes</label><asp:TextBox ID="ReadingMinutesTextBox" runat="server" TextMode="Number" /></div>
                <div><label>Concept Coverage %</label><asp:TextBox ID="CoverageTextBox" runat="server" TextMode="Number" /></div>
            </div>
            <div class="upload-row">
                <label>Tags</label>
                <div class="tag-entry">
                    <input type="text" id="TagSearchInput" class="auth-input input-strong" list="TagSuggestions" placeholder="Type a tag and add it" />
                    <button type="button" class="btn btn-secondary" onclick="return edxAddTag();">Add Tag</button>
                </div>
                <datalist id="TagSuggestions">
                    <asp:Literal ID="TagSuggestionLiteral" runat="server" />
                </datalist>
                <div id="SelectedTags" class="tag-bag"></div>
                <asp:TextBox ID="TagsTextBox" runat="server" Style="display:none;" />
            </div>
            <div class="upload-row">
                <label>Note File</label>
                <asp:FileUpload ID="NoteFileUpload" runat="server" />
            </div>
            <div class="table-actions upload-actions">
                <asp:Button ID="UploadButton" runat="server" Text="Submit for Approval" OnClick="UploadButton_Click" />
            </div>
        </div>
    </div>
</asp:Content>
