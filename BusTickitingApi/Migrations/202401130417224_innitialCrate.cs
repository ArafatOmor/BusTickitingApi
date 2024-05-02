namespace BusTickitingApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class innitialCrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        PurchaseNo = c.String(),
                        PassangerName = c.String(),
                        PurchaseDate = c.DateTime(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        ImageUrl = c.String(),
                    })
                .PrimaryKey(t => t.PurchaseId);
            
            CreateTable(
                "dbo.PurchaseTickits",
                c => new
                    {
                        PurchaseTickitId = c.Int(nullable: false, identity: true),
                        PurchaseId = c.Int(nullable: false),
                        SeatTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseTickitId)
                .ForeignKey("dbo.SeatTypes", t => t.SeatTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Purchases", t => t.PurchaseId, cascadeDelete: true)
                .Index(t => t.PurchaseId)
                .Index(t => t.SeatTypeId);
            
            CreateTable(
                "dbo.SeatTypes",
                c => new
                    {
                        SeatTypeId = c.Int(nullable: false, identity: true),
                        TypeName = c.String(),
                        SeatFear = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.SeatTypeId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                        Roles = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PurchaseTickits", "PurchaseId", "dbo.Purchases");
            DropForeignKey("dbo.PurchaseTickits", "SeatTypeId", "dbo.SeatTypes");
            DropIndex("dbo.PurchaseTickits", new[] { "SeatTypeId" });
            DropIndex("dbo.PurchaseTickits", new[] { "PurchaseId" });
            DropTable("dbo.Users");
            DropTable("dbo.SeatTypes");
            DropTable("dbo.PurchaseTickits");
            DropTable("dbo.Purchases");
        }
    }
}
