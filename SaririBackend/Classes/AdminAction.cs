namespace SaririBackend.Classes
{
    public class AdminAction
    {
        public int Id { get; set; } // the action ID
        public int AdminId { get; set; } // the ID of the Admin
        public string ActionType { get; set; } // the Type of action

        public DateTime TimeStamp { get; set; } // the time of the action
    }
}
