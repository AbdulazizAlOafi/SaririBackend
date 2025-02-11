using System.ComponentModel.DataAnnotations;

namespace SaririBackend.Classes
{
    public class User
    {
        [Key]
        public int userID { get; set; } // User ID
        public string userName { get; set; } // User name
        public string email { get; set; } // The user email
        public string password { get; set; } // user password
        public string role { get; set; } // the role of the user
    }
}
