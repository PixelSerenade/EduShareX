using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EduX
{
    public static class EduShareRepository
    {
        private const string UsersKey = "EduShareX.Users";
        private const string NotesKey = "EduShareX.Notes";
        private const string NextNoteIdKey = "EduShareX.NextNoteId";
        private static readonly object SyncRoot = new object();
        private static readonly string[] AvailableSubjects =
        {
            "DBMS",
            "Operating Systems",
            "Computer Networks",
            "Software Engineering",
            "Data Structures",
            "Algorithms",
            "Machine Learning",
            "Artificial Intelligence",
            "Web Technology",
            "Compiler Design",
            "Cloud Computing",
            "Cyber Security"
        };
        private static readonly string[] AvailableDepartments =
        {
            "Computer Science",
            "Information Technology",
            "Electronics and Communication",
            "Electrical Engineering",
            "Mechanical Engineering",
            "Civil Engineering",
            "Artificial Intelligence and Data Science"
        };
        private static readonly string[] AvailableCourses =
        {
            "B.Tech CSE",
            "B.Tech IT",
            "B.Tech AI",
            "B.Tech ECE",
            "B.Tech EEE",
            "B.Tech Mechanical"
        };
        private static readonly string[] AvailableTags =
        {
            "Exam Prep",
            "Unit Notes",
            "Important Questions",
            "Quick Revision",
            "Handwritten",
            "Lecture Summary",
            "Solved Problems",
            "Lab Reference",
            "Formulas",
            "Viva Preparation",
            "Previous Year Topics",
            "Cheat Sheet"
        };

        public static void Initialize()
        {
            var app = HttpContext.Current.Application;
            if (app[UsersKey] != null)
            {
                return;
            }

            lock (SyncRoot)
            {
                if (app[UsersKey] != null)
                {
                    return;
                }

                var users = new List<PlatformUser>();
                var notes = new List<AcademicNote>();

                app[UsersKey] = users;
                app[NotesKey] = notes;
                app[NextNoteIdKey] = 1;
            }
        }

        public static IEnumerable<string> GetAvailableSubjects()
        {
            return AvailableSubjects;
        }

        public static IEnumerable<string> GetAvailableDepartments()
        {
            return AvailableDepartments;
        }

        public static IEnumerable<string> GetAvailableCourses()
        {
            return AvailableCourses;
        }

        public static IEnumerable<string> GetAvailableTags()
        {
            return AvailableTags;
        }

        public static List<PlatformUser> GetUsers()
        {
            Initialize();
            return (List<PlatformUser>)HttpContext.Current.Application[UsersKey];
        }

        public static List<AcademicNote> GetNotes()
        {
            Initialize();
            return (List<AcademicNote>)HttpContext.Current.Application[NotesKey];
        }

        public static PlatformUser GetUserByEmail(string email)
        {
            return GetUsers().FirstOrDefault(item => item.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public static PlatformUser ValidateUser(string email, string password)
        {
            return GetUsers().FirstOrDefault(item => item.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && item.Password == password);
        }

        public static IEnumerable<PlatformUser> GetLecturers()
        {
            return GetUsers().Where(item => item.Role == UserRole.Lecturer).ToList();
        }

        public static bool RegisterUser(PlatformUser user, out string errorMessage)
        {
            lock (SyncRoot)
            {
                if (GetUserByEmail(user.Email) != null)
                {
                    errorMessage = "An account with this email already exists.";
                    return false;
                }

                GetUsers().Add(user);
                errorMessage = string.Empty;
                return true;
            }
        }

        public static IEnumerable<AcademicNote> SearchNotes(string subject, string keyword)
        {
            IEnumerable<AcademicNote> query = GetNotes().Where(item => item.IsApproved && !item.IsRejected);

            if (!string.IsNullOrWhiteSpace(subject))
            {
                query = query.Where(item => item.Subject.IndexOf(subject, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(item => item.Title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 || item.Tags.Any(tag => tag.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0));
            }

            return query.OrderByDescending(item => item.SmartQualityScore).ToList();
        }

        public static AcademicNote GetNote(int id)
        {
            return GetNotes().FirstOrDefault(item => item.Id == id);
        }

        public static DashboardStats GetStats()
        {
            var notes = GetNotes();
            return new DashboardStats
            {
                TotalUsers = GetUsers().Count,
                TotalNotes = notes.Count,
                PendingNotes = notes.Count(item => !item.IsApproved && !item.IsRejected),
                ApprovedNotes = notes.Count(item => item.IsApproved),
                TotalDownloads = notes.Sum(item => item.DownloadCount),
                TotalReports = notes.Sum(item => item.Reports.Count)
            };
        }

        public static AcademicNote AddNote(PlatformUser user, string title, string subject, int semester, string course, string difficulty, string contentType, int readingMinutes, int coveragePercent, string tags, HttpPostedFile postedFile)
        {
            lock (SyncRoot)
            {
                var nextId = (int)HttpContext.Current.Application[NextNoteIdKey];
                HttpContext.Current.Application[NextNoteIdKey] = nextId + 1;
                var assignedLecturer = FindLecturerForSubject(subject);

                var uploadsFolder = HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var extension = Path.GetExtension(postedFile.FileName);
                var storedFileName = "note_" + nextId + "_" + Guid.NewGuid().ToString("N") + extension;
                postedFile.SaveAs(Path.Combine(uploadsFolder, storedFileName));

                var note = new AcademicNote
                {
                    Id = nextId,
                    Title = title,
                    Subject = subject,
                    Semester = semester,
                    Course = course,
                    Difficulty = difficulty,
                    ContentType = contentType,
                    ContributorEmail = user.Email,
                    ContributorName = user.FullName,
                    ContributorDepartment = user.Department,
                    ContributorRole = user.Role,
                    AssignedLecturerEmail = assignedLecturer != null ? assignedLecturer.Email : string.Empty,
                    AssignedLecturerName = assignedLecturer != null ? assignedLecturer.FullName : "Unassigned Lecturer",
                    FileName = Path.GetFileName(postedFile.FileName),
                    StoredFileName = storedFileName,
                    FileExtension = extension,
                    EstimatedReadingMinutes = readingMinutes,
                    ConceptCoveragePercent = coveragePercent,
                    CreatedOn = DateTime.Now,
                    Tags = tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).Where(item => item.Length > 0).ToList()
                };

                GetNotes().Add(note);
                return note;
            }
        }

        public static IEnumerable<AcademicNote> GetNotesByContributor(string email)
        {
            return GetNotes().Where(item => item.ContributorEmail.Equals(email, StringComparison.OrdinalIgnoreCase)).OrderByDescending(item => item.CreatedOn).ToList();
        }

        public static IEnumerable<AcademicNote> GetBookmarkedNotes(string email)
        {
            return GetNotes().Where(item => item.BookmarkEmails.Contains(email)).OrderByDescending(item => item.SmartQualityScore).ToList();
        }

        public static IEnumerable<Tuple<AcademicNote, RatingEntry>> GetRatingsByUser(string email)
        {
            return GetNotes()
                .SelectMany(note => note.Ratings.Where(rating => rating.UserEmail.Equals(email, StringComparison.OrdinalIgnoreCase)).Select(rating => Tuple.Create(note, rating)))
                .ToList();
        }

        public static void ToggleBookmark(int noteId, string email)
        {
            lock (SyncRoot)
            {
                var note = GetNote(noteId);
                if (note == null)
                {
                    return;
                }

                if (note.BookmarkEmails.Contains(email))
                {
                    note.BookmarkEmails.Remove(email);
                    AdjustContributorReputation(note.ContributorEmail, -2);
                }
                else
                {
                    note.BookmarkEmails.Add(email);
                    AdjustContributorReputation(note.ContributorEmail, 2);
                }
            }
        }

        public static void AddComment(int noteId, string authorName, string message)
        {
            lock (SyncRoot)
            {
                var note = GetNote(noteId);
                if (note == null)
                {
                    return;
                }

                note.Comments.Add(new CommentEntry
                {
                    AuthorName = authorName,
                    Message = message,
                    CreatedOn = DateTime.Now
                });
            }
        }

        public static void AddOrUpdateRating(int noteId, string email, int score)
        {
            lock (SyncRoot)
            {
                var note = GetNote(noteId);
                if (note == null)
                {
                    return;
                }

                var existing = note.Ratings.FirstOrDefault(item => item.UserEmail.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    note.Ratings.Add(new RatingEntry { UserEmail = email, Score = score });
                    AdjustContributorReputation(note.ContributorEmail, ReputationDelta(score));
                }
                else
                {
                    AdjustContributorReputation(note.ContributorEmail, ReputationDelta(score) - ReputationDelta(existing.Score));
                    existing.Score = score;
                }
            }
        }

        public static void AddReport(int noteId, string email, string reporterName, string reason)
        {
            lock (SyncRoot)
            {
                var note = GetNote(noteId);
                if (note == null || note.Reports.Any(item => item.UserEmail.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    return;
                }

                note.Reports.Add(new ReportEntry
                {
                    UserEmail = email,
                    ReporterName = reporterName,
                    Reason = reason,
                    CreatedOn = DateTime.Now
                });
            }
        }

        public static void ApproveNote(int noteId)
        {
            ApproveNote(noteId, string.Empty);
        }

        public static void ApproveNote(int noteId, string lecturerEmail)
        {
            lock (SyncRoot)
            {
                var note = GetNote(noteId);
                if (note == null || note.IsApproved || !CanReviewNote(note, lecturerEmail))
                {
                    return;
                }

                note.IsApproved = true;
                note.IsRejected = false;
                note.IsPeerVerified = true;
                note.ReviewedByLecturerName = GetUserByEmail(lecturerEmail) != null ? GetUserByEmail(lecturerEmail).FullName : note.ReviewedByLecturerName;
                AdjustContributorReputation(note.ContributorEmail, 20);
            }
        }

        public static void RejectNote(int noteId)
        {
            RejectNote(noteId, string.Empty);
        }

        public static void RejectNote(int noteId, string lecturerEmail)
        {
            lock (SyncRoot)
            {
                var note = GetNote(noteId);
                if (note == null || note.IsRejected || !CanReviewNote(note, lecturerEmail))
                {
                    return;
                }

                note.IsApproved = false;
                note.IsRejected = true;
                note.ReviewedByLecturerName = GetUserByEmail(lecturerEmail) != null ? GetUserByEmail(lecturerEmail).FullName : note.ReviewedByLecturerName;
                AdjustContributorReputation(note.ContributorEmail, -15);
            }
        }

        public static void IncrementDownload(int noteId)
        {
            lock (SyncRoot)
            {
                var note = GetNote(noteId);
                if (note != null)
                {
                    note.DownloadCount += 1;
                }
            }
        }

        public static void UpdateProfile(string email, string fullName, string department, string subjectExpertise)
        {
            lock (SyncRoot)
            {
                var user = GetUserByEmail(email);
                if (user == null)
                {
                    return;
                }

                user.FullName = fullName;
                user.Department = department;
                user.SubjectExpertise = subjectExpertise;

                foreach (var note in GetNotes().Where(item => item.ContributorEmail.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    note.ContributorName = fullName;
                    note.ContributorDepartment = department;
                }

                foreach (var note in GetNotes().Where(item => item.AssignedLecturerEmail.Equals(email, StringComparison.OrdinalIgnoreCase)))
                {
                    note.AssignedLecturerName = fullName;
                }
            }
        }

        public static IEnumerable<AcademicNote> GetPendingNotes(string lecturerEmail)
        {
            return GetNotes()
                .Where(item => !item.IsApproved && !item.IsRejected && CanReviewNote(item, lecturerEmail))
                .OrderByDescending(item => item.CreatedOn)
                .ToList();
        }

        public static IEnumerable<AcademicNote> GetReportedNotes(string lecturerEmail)
        {
            return GetNotes()
                .Where(item => item.Reports.Count > 0 && CanReviewNote(item, lecturerEmail))
                .OrderByDescending(item => item.Reports.Count)
                .ToList();
        }

        public static IEnumerable<AcademicNote> GetFlaggedNotes()
        {
            return GetNotes().Where(item => item.AverageRating > 0 && item.AverageRating < 4.0).OrderBy(item => item.AverageRating).ToList();
        }

        private static void AdjustContributorReputation(string email, int delta)
        {
            var user = GetUserByEmail(email);
            if (user != null)
            {
                user.ReputationPoints = Math.Max(0, user.ReputationPoints + delta);
            }
        }

        private static int ReputationDelta(int score)
        {
            switch (score)
            {
                case 5: return 6;
                case 4: return 3;
                case 3: return 1;
                case 2: return -2;
                case 1: return -4;
                default: return 0;
            }
        }

        private static PlatformUser FindLecturerForSubject(string subject)
        {
            var normalizedSubject = (subject ?? string.Empty).Trim();
            var lecturers = GetLecturers().ToList();
            if (lecturers.Count == 0)
            {
                return null;
            }

            var exactMatches = lecturers.Where(item =>
                !string.IsNullOrWhiteSpace(item.SubjectExpertise) &&
                item.SubjectExpertise.Split(',')
                    .Select(value => value.Trim())
                    .Any(value => normalizedSubject.Equals(value, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            if (exactMatches.Count > 0)
            {
                return exactMatches
                    .OrderBy(item => GetPendingNotes(item.Email).Count())
                    .ThenBy(item => item.FullName)
                    .First();
            }

            var partialMatches = lecturers.Where(item =>
                !string.IsNullOrWhiteSpace(item.SubjectExpertise) &&
                item.SubjectExpertise.IndexOf(normalizedSubject, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            if (partialMatches.Count > 0)
            {
                return partialMatches
                    .OrderBy(item => GetPendingNotes(item.Email).Count())
                    .ThenBy(item => item.FullName)
                    .First();
            }

            return lecturers
                .OrderBy(item => GetPendingNotes(item.Email).Count())
                .ThenBy(item => item.FullName)
                .First();
        }

        private static bool CanReviewNote(AcademicNote note, string lecturerEmail)
        {
            if (string.IsNullOrWhiteSpace(lecturerEmail))
            {
                return true;
            }

            return string.Equals(note.AssignedLecturerEmail ?? string.Empty, lecturerEmail, StringComparison.OrdinalIgnoreCase);
        }
    }
}
