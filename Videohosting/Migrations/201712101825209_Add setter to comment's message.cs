namespace Videohosting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addsettertocommentsmessage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "Message");
        }
    }
}
