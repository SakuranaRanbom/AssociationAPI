
using Microsoft.EntityFrameworkCore;
using RfidAPI.Models;

namespace RfidAPI.Data
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }
        
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }
        
        
        
        //public DbSet<DeviceCount> DeviceCounts { get; set; }
    }
}