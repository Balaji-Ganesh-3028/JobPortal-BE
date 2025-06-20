﻿namespace JobPortal.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Salutation { get; set; }
        public string Gender { get; set; }
        public List<MastersList> Interests { get; set; }
        public List<EducationInformation> EducationInformation { get; set; }
        public List<ExperienceInformation> ExperienceInformation { get; set; }
        public List<Address> Address { get; set; }
    }
}
