using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Data
{
    public class DentistBookingContext : DbContext
    {
        public DentistBookingContext() { }
        public DentistBookingContext(DbContextOptions<DentistBookingContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("LocalConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Admin> Admins {get; set;}
        public DbSet<User> Users {get; set;}
        public DbSet<Dentist> Dentists {get; set;}
        public DbSet<Customer> Customers { get; set;}
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set;}
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }


    }
}
