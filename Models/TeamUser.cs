namespace RfidAPI.Models
{

    public enum PerLevel
    {
        not,
        editor,
        admin
    }
    
    public class TeamUser
    {
        
        public string? TeamUserID { get; set; }
        
        public string UserID { get; set; }
        
        public int TeamID { get; set; } 
        
        public PerLevel level { get; set; }
    }
}