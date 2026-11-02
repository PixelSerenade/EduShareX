namespace EduX
{
    public class PlatformUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string SubjectExpertise { get; set; }
        public string ProfilePhotoPath { get; set; }
        public UserRole Role { get; set; }
        public int ReputationPoints { get; set; }
    }
}
