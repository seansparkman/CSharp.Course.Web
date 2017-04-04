namespace CSharp.Course.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubmissionDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoardEntries", "Submitted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BoardEntries", "Submitted");
        }
    }
}
