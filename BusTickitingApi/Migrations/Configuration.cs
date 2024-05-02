namespace BusTickitingApi.Migrations
{
    using BusTickitingApi.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BusTickitingApi.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BusTickitingApi.Models.AppDbContext context)
        {
            context.SeatTypes.AddOrUpdate(x => x.SeatTypeId,
              new Models.SeatType() { SeatTypeId = 1, TypeName = "Hanif", SeatFear = 89000 },
              new Models.SeatType() { SeatTypeId = 2, TypeName = "Nabil", SeatFear = 18000 },
              new Models.SeatType() { SeatTypeId = 3, TypeName = "Ena", SeatFear = 35000 });

            context.Users.AddOrUpdate(x => x.UserId,
                new Models.User() { UserId = 1, UserName = "Admin", Password = "1234", Email = "admin@gmail.com", Roles = "Admin" },
                new Models.User() { UserId = 2, UserName = "User", Password = "1234", Email = "User@gmail.com", Roles = "User" });
        }
    }
}
