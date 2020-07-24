using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginMicroservice.Database.Entities;

namespace LoginMicroservice.Database
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=LAPTOP-5AHB7Q4H\SQLEXPRESS;initial catalog=MicroserviceAssignment;integrated security=True;MultipleActiveResultSets=True;");
        }
        public DbSet<User> Users { get; set; }
    }
}
