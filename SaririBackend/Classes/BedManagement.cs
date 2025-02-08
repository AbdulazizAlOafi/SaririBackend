namespace SaririBackend.Classes
{
    public class BedManagement
    {
        public int Id { get; set; } // the ID of the bed
        public int HospitalId {  get; set; } // the ID of the hospital the bed is in
        public Boolean Condition { get; set; } // the condition of the bed true==available false==not available

        public DateTime LastUpdated { get; set; } // the last time the bed was updated
    }
}
