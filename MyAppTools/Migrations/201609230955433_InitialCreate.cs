namespace MyAppTools.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiEndpoints",
                c => new
                    {
                        apiEndpointsId = c.Int(nullable: false, identity: true),
                        absolutePath = c.String(),
                        dateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.apiEndpointsId);
            
            CreateTable(
                "dbo.ClientPermissions",
                c => new
                    {
                        ClientPermissionsId = c.Int(nullable: false, identity: true),
                        clientID = c.String(),
                        endPointId = c.Int(nullable: false),
                        dateAdded = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClientPermissionsId);
            
            CreateTable(
                "dbo.ClientProfiles",
                c => new
                    {
                        ClientProfileId = c.Int(nullable: false, identity: true),
                        clientId = c.String(),
                        clientKey = c.String(),
                        clientIpAddress = c.String(),
                        clientDescription = c.String(),
                        unRestricted = c.Boolean(nullable: false),
                        status = c.String(),
                        dateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClientProfileId);
            
            CreateTable(
                "dbo.EventLogs",
                c => new
                    {
                        eventLogId = c.Int(nullable: false, identity: true),
                        clientID = c.String(),
                        clientIpAddress = c.String(),
                        endPoint = c.String(),
                        eventSource = c.String(),
                        requestTime = c.DateTime(nullable: false),
                        requestContent = c.String(),
                        responseContent = c.String(),
                        responseCode = c.String(),
                        reponseTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.eventLogId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EventLogs");
            DropTable("dbo.ClientProfiles");
            DropTable("dbo.ClientPermissions");
            DropTable("dbo.ApiEndpoints");
        }
    }
}
