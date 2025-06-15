using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ReportPostForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Posts_PostId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_PostId",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Report");

            migrationBuilder.CreateIndex(
                name: "IX_Report_ReportedPostId",
                table: "Report",
                column: "ReportedPostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Posts_ReportedPostId",
                table: "Report",
                column: "ReportedPostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Posts_ReportedPostId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_ReportedPostId",
                table: "Report");

            migrationBuilder.AddColumn<Guid>(
                name: "PostId",
                table: "Report",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_PostId",
                table: "Report",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Posts_PostId",
                table: "Report",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
