using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TeamAPI.Models;

namespace TeamAPI.Data
{
    public class LibraryDbContext : IdentityDbContext<IUser, IRole, string>
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
            
        }
        public DbSet<IUser> IUsers { get; set; }
        public DbSet<IRole> Iroles { get; set; }
        
        /*
        public DbSet<Repo> Repos { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Log>  Logs { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        */
        
        
        
        
    }
}