using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticlesApp.Database.SqlServer._Migrations
{
    public partial class Add_VersionId_To_Articles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "VersionId",
                table: "Articles",
                type: "smallint",
                rowVersion: true,
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "Articles");
        }
    }
}
