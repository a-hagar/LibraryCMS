namespace LibraryCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookslocations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "Book_BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "Location_LocationId", "dbo.Locations");
            DropIndex("dbo.Books", new[] { "Book_BookId" });
            DropIndex("dbo.Books", new[] { "Location_LocationId" });
            CreateTable(
                "dbo.BookLocations",
                c => new
                    {
                        Book_BookId = c.Int(nullable: false),
                        Location_LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_BookId, t.Location_LocationId })
                .ForeignKey("dbo.Books", t => t.Book_BookId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId, cascadeDelete: true)
                .Index(t => t.Book_BookId)
                .Index(t => t.Location_LocationId);
            
            DropColumn("dbo.Books", "Book_BookId");
            DropColumn("dbo.Books", "Location_LocationId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Location_LocationId", c => c.Int());
            AddColumn("dbo.Books", "Book_BookId", c => c.Int());
            DropForeignKey("dbo.BookLocations", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.BookLocations", "Book_BookId", "dbo.Books");
            DropIndex("dbo.BookLocations", new[] { "Location_LocationId" });
            DropIndex("dbo.BookLocations", new[] { "Book_BookId" });
            DropTable("dbo.BookLocations");
            CreateIndex("dbo.Books", "Location_LocationId");
            CreateIndex("dbo.Books", "Book_BookId");
            AddForeignKey("dbo.Books", "Location_LocationId", "dbo.Locations", "LocationId");
            AddForeignKey("dbo.Books", "Book_BookId", "dbo.Books", "BookId");
        }
    }
}
