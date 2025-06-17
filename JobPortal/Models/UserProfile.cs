namespace JobPortal.Models
{
    public class UserProfile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Salutation { get; set; }
        public string Gender { get; set; }
        public List<string> Interests { get; set; }
        public List<EducationInformation> EducationInformation { get; set; }
        public List<ExperienceInformation> ExperienceInformation { get; set; }
        public List<Address> Address { get; set; }
    }
}
