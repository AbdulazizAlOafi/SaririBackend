using System.ComponentModel.DataAnnotations;

namespace SaririBackend.Classes
{
    public class AdminAction
    {
        [Key]
        public int actionID { get; set; } // the action ID
        public int adminID { get; set; } // the ID of the Admin
        public string actionType { get; set; } // the Type of action

        public DateTime timestamp { get; set; } // the time of the action
    }
}
