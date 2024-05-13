using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArabTube.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddingWatchedVideosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WatchedVideos",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WatchedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedVideos", x => new { x.UserId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_WatchedVideos_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_WatchedVideos_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchedVideos_VideoId",
                table: "WatchedVideos",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchedVideos");
        }
    }
}
