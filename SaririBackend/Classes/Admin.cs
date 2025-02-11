using System.ComponentModel.DataAnnotations;

namespace SaririBackend.Classes
{
    public class Admin
    {
        [Key]
        public int adminID { get; set; } // the admin ID
        public int userID { get; set; } // the User ID
    }
}
