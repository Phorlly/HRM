namespace HRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Employee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(maxLength: 100, nullable: true),
                    LastName = c.String(maxLength: 100, nullable: true),
                    Code = c.String(maxLength: 50, nullable: true),
                    Sex = c.Boolean(nullable: false),
                    Email = c.String(maxLength: 100, nullable: true),
                    Phone = c.String(maxLength: 15, nullable: true),
                    Position = c.String(maxLength: 50, nullable: true),
                    Address = c.String(maxLength: 255, nullable: true),
                    PlaceOfBirth = c.String(maxLength: 255, nullable: true),
                    Profile = c.String(maxLength: 100, nullable: true),
                    StartedAt = c.DateTime(),
                    EndedAt = c.DateTime(),
                    DateOfBirth = c.DateTime(),
                    Age = c.Int(nullable: true),
                    CurrencyId = c.Int(nullable: true),
                    InitialSalary = c.Decimal(precision: 18, scale: 2),
                    CreatedAt = c.DateTime(),
                    UpdatedAt = c.DateTime(),
                    DeletedAt = c.DateTime(),
                    Status = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId, cascadeDelete: true)
                .Index(t => t.CurrencyId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Employees", "CurrencyId", "dbo.Currencies");
            DropIndex("dbo.Employees", new[] { "CurrencyId" });
            DropTable("dbo.Employees");
        }
    }
}
