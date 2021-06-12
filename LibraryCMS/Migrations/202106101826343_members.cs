namespace LibraryCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class members : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.BookLocations", newName: "LocationBooks");
            DropPrimaryKey("dbo.LocationBooks");
            AddPrimaryKey("dbo.LocationBooks", new[] { "Location_LocationId", "Book_BookId" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.LocationBooks");
            AddPrimaryKey("dbo.LocationBooks", new[] { "Book_BookId", "Location_LocationId" });
            RenameTable(name: "dbo.LocationBooks", newName: "BookLocations");
        }
    }
}
