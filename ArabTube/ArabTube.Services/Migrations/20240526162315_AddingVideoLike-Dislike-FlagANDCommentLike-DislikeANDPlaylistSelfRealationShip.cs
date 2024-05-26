using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArabTube.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddingVideoLikeDislikeFlagANDCommentLikeDislikeANDPlaylistSelfRealationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentPlaylistId",
                table: "Playlists",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CommentsDislikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsDislikes", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommentsDislikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CommentsDislikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentsLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommentId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentsLikes", x => new { x.CommentId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommentsLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_CommentsLikes_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideosDislikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideosDislikes", x => new { x.UserId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_VideosDislikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VideosDislikes_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideosFlags",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideosFlags", x => new { x.UserId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_VideosFlags_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_VideosFlags_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideosLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideosLikes", x => new { x.UserId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_VideosLikes_AspNetUsers_UserId",
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
                name: "IX_Playlists_ParentPlaylistId",
                table: "Playlists",
                column: "ParentPlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsDislikes_UserId",
                table: "CommentsDislikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentsLikes_UserId",
                table: "CommentsLikes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VideosDislikes_VideoId",
                table: "VideosDislikes",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideosFlags_VideoId",
                table: "VideosFlags",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_VideosLikes_VideoId",
                table: "VideosLikes",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_Playlists_ParentPlaylistId",
                table: "Playlists",
                column: "ParentPlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_Playlists_ParentPlaylistId",
                table: "Playlists");

            migrationBuilder.DropTable(
                name: "CommentsDislikes");

            migrationBuilder.DropTable(
                name: "CommentsLikes");

            migrationBuilder.DropTable(
                name: "VideosDislikes");

            migrationBuilder.DropTable(
                name: "VideosFlags");

            migrationBuilder.DropTable(
                name: "VideosLikes");

            migrationBuilder.DropIndex(
                name: "IX_Playlists_ParentPlaylistId",
                table: "Playlists");

            migrationBuilder.DropColumn(
                name: "ParentPlaylistId",
                table: "Playlists");
        }
    }
}
