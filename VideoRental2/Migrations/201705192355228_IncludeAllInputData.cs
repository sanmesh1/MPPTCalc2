namespace VideoRental2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncludeAllInputData : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO MembershipTypes (id, signUpFee, durationInMonths, discountRate) VALUES (1,0,0,0)");
            Sql("INSERT INTO MembershipTypes (id, signUpFee, durationInMonths, discountRate) VALUES (2,30,1,10)");
            Sql("INSERT INTO MembershipTypes (id, signUpFee, durationInMonths, discountRate) VALUES (3,90,3,15)");
            Sql("INSERT INTO MembershipTypes (id, signUpFee, durationInMonths, discountRate) VALUES (4,300,3,15)");
            Sql("UPDATE MembershipTypes SET name = 'Pay as you go' WHERE id = 1");
            Sql("UPDATE MembershipTypes SET name = 'Monthly' WHERE id = 2");
            Sql("UPDATE MembershipTypes SET name = 'Every 3 months' WHERE id = 3");
            Sql("UPDATE MembershipTypes SET name = 'Every 10 months' WHERE id = 4");
            Sql("INSERT INTO Genres (id, name) VALUES (1,'Comedy')");
            Sql("INSERT INTO Genres (id, name) VALUES (2,'Action')");
            Sql("INSERT INTO Genres (id, name) VALUES (3,'Adventure')");
            Sql("INSERT INTO Genres (id, name) VALUES (4,'Romance')");
            Sql("INSERT INTO Genres (id, name) VALUES (5,'Mystery')");
        }
        
        public override void Down()
        {
        }
    }
}
