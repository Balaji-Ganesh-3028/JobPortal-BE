namespace JobPortal.Models
{
    public class ExperienceInformationDto
    {
        public int Id { get; set; }
        public string Employer { get; set; }
        public string JobRole { get; set; }
        public DateOnly DOJ { get; set; } // DOJ = Date of Joining
        public int DurationInMonth { get; set; }
        public int UserId { get; set; }
    }
}
