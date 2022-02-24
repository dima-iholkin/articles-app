using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticlesApp.Database.SqlServer._Migrations;



public partial class AccountMerges_rename_columns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "SecondaryUserConfirm",
            table: "AccountMerges",
            newName: "SecondaryUserConfirmed");

        migrationBuilder.RenameColumn(
            name: "PrimaryUserConfirm",
            table: "AccountMerges",
            newName: "PrimaryUserConfirmed");
    }



    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "SecondaryUserConfirmed",
            table: "AccountMerges",
            newName: "SecondaryUserConfirm");

        migrationBuilder.RenameColumn(
            name: "PrimaryUserConfirmed",
            table: "AccountMerges",
            newName: "PrimaryUserConfirm");
    }
}