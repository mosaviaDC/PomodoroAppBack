using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PomodoroApp.Models
{ 
    public class EFDataContext : IdentityDbContext<User>
    {
        public DbSet<Tasks> Tasks { get; set; }

       
        public  EFDataContext(DbContextOptions<EFDataContext> dbContext): base(dbContext)
        {
      

        }

     
    }

}
