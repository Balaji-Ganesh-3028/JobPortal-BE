namespace JobPortal.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string Type { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string DoorNoStreet { get; set; }
        public string Pincode { get; set; }
    }
}
