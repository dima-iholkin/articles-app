using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArticlesApp.Database.SqlServer._Migrations;



public partial class Add_AccountMerges_table : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AccountMerges",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PrimaryUserId = table.Column<string>(type: "char(36)", nullable: false),
                SecondaryUserId = table.Column<string>(type: "char(36)", nullable: false),
                PrimaryUserConfirm = table.Column<bool>(type: "bit", nullable: false),
                SecondaryUserConfirm = table.Column<bool>(type: "bit", nullable: false),
                Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AccountMerges", x => x.Id);
                table.ForeignKey(
                    name: "FK_AccountMerges_AspNetUsers_PrimaryUserId",
                    column: x => x.PrimaryUserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_AccountMerges_AspNetUsers_SecondaryUserId",
                    column: x => x.SecondaryUserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_AccountMerges_PrimaryUserId",
            table: "AccountMerges",
            column: "PrimaryUserId");

        migrationBuilder.CreateIndex(
            name: "IX_AccountMerges_SecondaryUserId",
            table: "AccountMerges",
            column: "SecondaryUserId");
    }



    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AccountMerges");
    }
}