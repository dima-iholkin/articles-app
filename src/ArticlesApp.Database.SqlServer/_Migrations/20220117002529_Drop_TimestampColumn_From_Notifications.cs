using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticlesApp.Database.SqlServer._Migrations
{
    public partial class Drop_TimestampColumn_From_Notifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Notifications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Timestamp",
                table: "Notifications",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }
    }
}
