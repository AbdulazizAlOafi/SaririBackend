using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SaririBackend.Classes
{
    public class Hospital
    {
        [Key]
        public int hospitalID { get; set; } // Primary Key
        public string hospitalName { get; set; } // Hospital Name
        
        public string location { get; set; } // Hospital Location

        public int phoneNumber { get; set; } // Phone Number of the Hospital
        
        public int bedCapacity { get; set; } // The maximum bed capacity of  a hospital

        // Foreign Key
        public int? userID { get; set; } // the user Id to be associated with the hospital
    }
}
