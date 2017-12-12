namespace Videohosting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChanelentity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id" });
            RenameColumn(table: "dbo.Videos", name: "User_Id", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Videos", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
            CreateTable(
                "dbo.Chanels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            AddColumn("dbo.Videos", "Chanel_Id", c => c.Int());
            CreateIndex("dbo.Videos", "Chanel_Id");
            AddForeignKey("dbo.Videos", "Chanel_Id", "dbo.Chanels", "Id");
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Videos", "Chanel_Id", "dbo.Chanels");
            DropForeignKey("dbo.Chanels", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Videos", new[] { "Chanel_Id" });
            DropIndex("dbo.Chanels", new[] { "User_Id" });
            DropColumn("dbo.Videos", "Chanel_Id");
            DropTable("dbo.Chanels");
            RenameIndex(table: "dbo.Videos", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Videos", name: "ApplicationUser_Id", newName: "User_Id");
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
