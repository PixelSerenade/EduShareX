using System;
using System.Collections.Generic;
using System.Linq;

namespace EduX
{
    public class AcademicNote
    {
        public AcademicNote()
        {
            Tags = new List<string>();
            Comments = new List<CommentEntry>();
            Ratings = new List<RatingEntry>();
            Reports = new List<ReportEntry>();
            BookmarkEmails = new List<string>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public int Semester { get; set; }
        public string Course { get; set; }
        public string Difficulty { get; set; }
        public string ContentType { get; set; }
        public string ContributorEmail { get; set; }
        public string ContributorName { get; set; }
        public string ContributorDepartment { get; set; }
        public UserRole ContributorRole { get; set; }
        public string AssignedLecturerEmail { get; set; }
        public string AssignedLecturerName { get; set; }
        public string ReviewedByLecturerName { get; set; }
        public string FileName { get; set; }
        public string StoredFileName { get; set; }
        public string FileExtension { get; set; }
        public int EstimatedReadingMinutes { get; set; }
        public int ConceptCoveragePercent { get; set; }
        public int DownloadCount { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
        public bool IsPeerVerified { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<string> Tags { get; set; }
        public List<CommentEntry> Comments { get; set; }
        public List<RatingEntry> Ratings { get; set; }
        public List<ReportEntry> Reports { get; set; }
        public List<string> BookmarkEmails { get; set; }

        public double AverageRating
        {
            get
            {
                if (Ratings == null || Ratings.Count == 0)
                {
                    return 0;
                }

                return Math.Round(Ratings.Average(item => item.Score), 1);
            }
        }

        public double SmartQualityScore
        {
            get
            {
                return Math.Round((AverageRating * 18) + (DownloadCount * 0.05) + (IsPeerVerified ? 10 : 0) + (IsApproved ? 8 : 0), 1);
            }
        }
    }
}
