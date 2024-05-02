using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BusTickitingApi.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext() : base("AppDbContext") { }

        public DbSet<User> Users { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseTickit> PurchaseTickits { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }
    }


}