namespace Videohosting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCommentEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        User_Id = c.String(maxLength: 128),
                        Video_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Videos", t => t.Video_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Video_Id);
            
            AddColumn("dbo.Videos", "FilePath", c => c.String());
            AddColumn("dbo.Videos", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Videos", "User_Id");
            AddForeignKey("dbo.Videos", "User_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Videos", "UserId");
            DropColumn("dbo.Videos", "CommentsCount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Videos", "CommentsCount", c => c.Int(nullable: false));
            AddColumn("dbo.Videos", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Videos", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "Video_Id", "dbo.Videos");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "Video_Id" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Videos", new[] { "User_Id" });
            DropColumn("dbo.Videos", "User_Id");
            DropColumn("dbo.Videos", "FilePath");
            DropTable("dbo.Comments");
        }
    }
}
