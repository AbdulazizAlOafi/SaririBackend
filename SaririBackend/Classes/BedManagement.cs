using System.ComponentModel.DataAnnotations;

namespace SaririBackend.Classes
{
    public class bedManagement
    {
        [Key]
        public int bedID { get; set; } // the ID of the bed
        public int hospitalID {  get; set; } // the ID of the hospital the bed is in
        public Boolean condition { get; set; } // the condition of the bed true==available false==not available

        public DateTime lastUpdate { get; set; } // the last time the bed was updated
    }
}
