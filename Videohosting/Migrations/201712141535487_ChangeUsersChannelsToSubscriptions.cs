namespace Videohosting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUsersChannelsToSubscriptions : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Videos", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Videos", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Videos", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Videos", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Videos", "ApplicationUser_Id");
            AddForeignKey("dbo.Videos", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
