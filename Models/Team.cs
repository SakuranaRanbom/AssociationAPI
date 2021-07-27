using System.ComponentModel.DataAnnotations;

namespace RfidAPI.Models
{
    public enum TeamStatus
    {
        not,
        ok
    }
    public class Team
    {
        [Key]
        public int? teamID { get; set; }
        
        public string teamName { get; set; }
        
        public string teamInfo { get; set; }
        
        public string teamQQ { get; set; }

        public TeamStatus status { get; set; }
    }
}