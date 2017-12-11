namespace Videohosting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContentBytestoVideoentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "ContentBytes", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "ContentBytes");
        }
    }
}
