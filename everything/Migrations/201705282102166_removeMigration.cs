namespace everything.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeMigration : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Blogs", "Discriminator");
            DropColumn("dbo.Feedbacks", "UserPicture");
            DropColumn("dbo.Feedbacks", "UserName");
            DropColumn("dbo.Feedbacks", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Feedbacks", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Feedbacks", "UserName", c => c.String());
            AddColumn("dbo.Feedbacks", "UserPicture", c => c.String());
            AddColumn("dbo.Blogs", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
