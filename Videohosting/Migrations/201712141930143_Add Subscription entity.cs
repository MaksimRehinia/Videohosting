namespace Videohosting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubscriptionentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subscriber_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Channels", t => t.Subscriber_Id)
                .Index(t => t.Subscriber_Id);
            
            AddColumn("dbo.Channels", "Subscription_Id", c => c.Int());
            CreateIndex("dbo.Channels", "Subscription_Id");
            AddForeignKey("dbo.Channels", "Subscription_Id", "dbo.Subscriptions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "Subscriber_Id", "dbo.Channels");
            DropForeignKey("dbo.Channels", "Subscription_Id", "dbo.Subscriptions");
            DropIndex("dbo.Subscriptions", new[] { "Subscriber_Id" });
            DropIndex("dbo.Channels", new[] { "Subscription_Id" });
            DropColumn("dbo.Channels", "Subscription_Id");
            DropTable("dbo.Subscriptions");
        }
    }
}
