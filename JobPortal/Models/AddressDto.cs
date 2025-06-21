namespace JobPortal.Models
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string AddressType { get; set; }
        public string DoorNoStreet { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
        public string StateOrProvince { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
    }
}
