namespace everything.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class serverM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Blogs", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Feedbacks", "UserPicture", c => c.String());
            AddColumn("dbo.Feedbacks", "UserName", c => c.String());
            AddColumn("dbo.Feedbacks", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "Discriminator");
            DropColumn("dbo.Feedbacks", "UserName");
            DropColumn("dbo.Feedbacks", "UserPicture");
            DropColumn("dbo.Blogs", "Discriminator");
        }
    }
}
