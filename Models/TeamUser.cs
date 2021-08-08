using System.ComponentModel.DataAnnotations;

namespace TeamAPI.Models
{

    public enum PerLevel
    {
        not,
        editor,
        admin
    }
    
    public class TeamUser
    {
        [Key]
        public int? TeamUserID { get; set; }
        
        public string UserID { get; set; }
        
        public int TeamID { get; set; } 
        
        public PerLevel level { get; set; }
    }
}