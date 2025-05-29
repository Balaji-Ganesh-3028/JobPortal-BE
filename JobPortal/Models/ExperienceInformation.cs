namespace JobPortal.Models
{
    public class ExperienceInformation
    {
        public int ExperienceId { get; set; }
        public string Employer {  get; set; }
        public DateOnly DOJ { get; set; }
        public string Role { get; set; }
        public int DurationInMonth { get; set; }
    }
}
