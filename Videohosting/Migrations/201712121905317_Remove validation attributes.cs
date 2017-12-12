namespace Videohosting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removevalidationattributes : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Videos", "Title", c => c.String());
            AlterColumn("dbo.Videos", "Description", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Videos", "Description", c => c.String(nullable: false, maxLength: 500));
            AlterColumn("dbo.Videos", "Title", c => c.String(nullable: false, maxLength: 150));
        }
    }
}
