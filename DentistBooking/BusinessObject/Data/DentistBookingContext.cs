using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Data
{
    public class DentistBookingContext : DbContext
    {
        public DentistBookingContext(DbContextOptions<DentistBookingContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins {get; set;}
        public DbSet<User> Users {get; set;}
        public DbSet<Dentist> Dentists {get; set;}
        public DbSet<Customer> Customers { get; set;}
        public DbSet<Role> Roles { get; set; }
        public DbSet<Service> Services { get; set;}
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Reservation> Reservations { get; set; }


    }
}
