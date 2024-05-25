using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArabTube.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddingVideosLikesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideosLikes",
                columns: table => new
                {
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideosLikes", x => new { x.UserId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_VideosLikes_AspNetUsers_AppUserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VideosLikes_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VideosLikes_VideoId",
                table: "VideosLikes",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideosLikes");
        }
    }
}
