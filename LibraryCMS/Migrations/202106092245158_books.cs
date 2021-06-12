namespace LibraryCMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class books1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "Publisher", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Publisher");
        }
    }
}
