using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ADVA_Test.Migrations
{
    public partial class ChangeEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_Manager_ID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Manager_ID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Manager_ID",
                table: "Employees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Manager_ID",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Manager_ID",
                table: "Employees",
                column: "Manager_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_Manager_ID",
                table: "Employees",
                column: "Manager_ID",
                principalTable: "Employees",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
