namespace Videohosting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addlikeentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LikeValue = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                        Video_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .ForeignKey("dbo.Videos", t => t.Video_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Video_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "Video_Id", "dbo.Videos");
            DropForeignKey("dbo.Likes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Likes", new[] { "Video_Id" });
            DropIndex("dbo.Likes", new[] { "User_Id" });
            DropTable("dbo.Likes");
        }
    }
}
