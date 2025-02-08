using Microsoft.AspNetCore.Identity;

namespace SaririBackend.Classes
{
    public class Hospital
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // Hospital Name
        
        public string Location { get; set; } // Hospital Location

        public int PhoneNumber { get; set; } // Phone Number of the Hospital
        
        public int BedCapacity { get; set; } // The maximum bed capacity of  a hospital

        // Foreign Key
        public int? UserId { get; set; } // the user Id to be associated with the hospital
    }
}
