namespace BlogMe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedAdminUserAndAdminRole : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'1cfc21e3-ef8b-4690-b14f-921880bdd830', N'admin@admin.com', 0, N'ABNAYQQRzS920WbykXTQl/0+Xw6RbMKCQcy59eyIW0RlkAL7hmwh4ZtizBQ4T1ulVg==', N'9dd4b707-8729-4b7c-9635-c3104d570a44', NULL, 0, 0, NULL, 1, 0, N'admin@admin.com')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'83d22e62-dba2-4b12-b816-ca192b3a768a', N'Admin')
                INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1cfc21e3-ef8b-4690-b14f-921880bdd830', N'83d22e62-dba2-4b12-b816-ca192b3a768a')
            ");
        }
        
        public override void Down()
        {
            Sql(@"
                DELETE FROM [dbo].[AspNetUserRoles] WHERE [UserId] = N'1cfc21e3-ef8b-4690-b14f-921880bdd830'
                DELETE FROM [dbo].[AspNetRoles] WHERE [Id] = N'83d22e62-dba2-4b12-b816-ca192b3a768a'
                DELETE FROM [dbo].[AspNetUsers] WHERE [Id] = N'1cfc21e3-ef8b-4690-b14f-921880bdd830'
            
            ");
        }
    }
}
