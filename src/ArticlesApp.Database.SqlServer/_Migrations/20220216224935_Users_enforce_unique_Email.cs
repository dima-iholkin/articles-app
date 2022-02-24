using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticlesApp.Database.SqlServer._Migrations;



public partial class Users_enforce_unique_Email : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "EmailIndex",
            table: "AspNetUsers");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail",
            unique: true,
            filter: "[NormalizedEmail] IS NOT NULL");
    }



    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "EmailIndex",
            table: "AspNetUsers");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "AspNetUsers",
            column: "NormalizedEmail");
    }
}
