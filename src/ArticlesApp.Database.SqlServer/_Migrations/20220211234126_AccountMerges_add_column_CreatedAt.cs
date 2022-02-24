using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticlesApp.Database.SqlServer._Migrations;



public partial class AccountMerges_add_column_CreatedAt : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt_DateUtc",
            table: "AccountMerges",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }



    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreatedAt_DateUtc",
            table: "AccountMerges");
    }
}