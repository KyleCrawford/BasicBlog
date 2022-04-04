namespace BlogMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBlogModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Blogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 255),
                        CreatedOn = c.DateTime(nullable: false),
                        BlogText = c.String(nullable: false),
                        BlogOwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.BlogOwnerId)
                .Index(t => t.BlogOwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Blogs", "BlogOwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Blogs", new[] { "BlogOwnerId" });
            DropTable("dbo.Blogs");
        }
    }
}
