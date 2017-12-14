namespace Videohosting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameChannelentity : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Chanels", newName: "Channels");
            RenameColumn(table: "dbo.Videos", name: "Chanel_Id", newName: "Channel_Id");
            RenameIndex(table: "dbo.Videos", name: "IX_Chanel_Id", newName: "IX_Channel_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Videos", name: "IX_Channel_Id", newName: "IX_Chanel_Id");
            RenameColumn(table: "dbo.Videos", name: "Channel_Id", newName: "Chanel_Id");
            RenameTable(name: "dbo.Channels", newName: "Chanels");
        }
    }
}
