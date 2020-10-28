using System.ComponentModel.DataAnnotations;

namespace Core.Entitites.Identity
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }

        // These two prop help EF configure relationship between address and user (1 to 1)
        [Required]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}