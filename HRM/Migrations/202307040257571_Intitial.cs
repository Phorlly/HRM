namespace HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Intitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Currencies",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 100, nullable: true),
                    Code = c.String(maxLength: 50, nullable: true),
                    Symbol = c.String(maxLength: 10, nullable: true),
                    CreatedAt = c.DateTime(),
                    UpdatedAt = c.DateTime(),
                    DeletedAt = c.DateTime(),
                    Status = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Username = c.String(maxLength: 100, nullable: true),
                    Gender = c.Boolean(nullable: false),
                    Email = c.String(maxLength: 52, nullable: true),
                    Address = c.String(maxLength: 512, nullable: true),
                    Phone = c.String(maxLength: 12, nullable: true),
                    Photo = c.String(maxLength: 255, nullable: true),
                    Password = c.String(maxLength: 255, nullable: true),
                    CreatedAt = c.DateTime(),
                    UpdatedAt = c.DateTime(),
                    IsAdmin = c.Boolean(nullable: false),
                    Status = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Currencies");
        }
    }
}
