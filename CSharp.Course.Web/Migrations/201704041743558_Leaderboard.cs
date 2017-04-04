namespace CSharp.Course.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Leaderboard : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoardEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(),
                        Passed = c.Int(nullable: false),
                        Failed = c.Int(nullable: false),
                        Skipped = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BoardEntries");
        }
    }
}
