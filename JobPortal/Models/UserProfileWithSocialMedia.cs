namespace JobPortal.Models
{
    public class UserProfileWithSocialMedia : UserProfile
    {
        public string GoogleLink { get; set; } = null;
        public string LinkedIn { get; set; }
    }
}
