namespace LibraryCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class memberslocations : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Members", "LocationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Members", "LocationId");
            AddForeignKey("dbo.Members", "LocationId", "dbo.Locations", "LocationId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Members", "LocationId", "dbo.Locations");
            DropIndex("dbo.Members", new[] { "LocationId" });
            DropColumn("dbo.Members", "LocationId");
        }
    }
}
