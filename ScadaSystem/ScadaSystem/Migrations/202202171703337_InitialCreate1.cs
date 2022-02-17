namespace ScadaSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlarmValues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        Value = c.Double(nullable: false),
                        Type = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        Limit = c.Double(nullable: false),
                        TagName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AlarmValues");
        }
    }
}
