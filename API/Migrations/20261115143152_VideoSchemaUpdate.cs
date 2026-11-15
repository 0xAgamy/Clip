using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clips_api.Migrations
{
    /// <inheritdoc />
    public partial class VideoSchemaUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "thumbnail",
                table: "Videos");

            migrationBuilder.RenameColumn(
                name: "videoUrl",
                table: "Videos",
                newName: "VideoUrl");

            migrationBuilder.RenameColumn(
                name: "thumbnailUrl",
                table: "Videos",
                newName: "ThumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "created",
                table: "Videos",
                newName: "Created");

            migrationBuilder.AlterColumn<string>(
                name: "VideoUrl",
                table: "Videos",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ThumbnailUrl",
                table: "Videos",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Videos",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Videos",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoUrl",
                table: "Videos",
                newName: "videoUrl");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                table: "Videos",
                newName: "thumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Videos",
                newName: "created");

            migrationBuilder.AlterColumn<string>(
                name: "videoUrl",
                table: "Videos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Videos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Videos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "thumbnailUrl",
                table: "Videos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AddColumn<string>(
                name: "thumbnail",
                table: "Videos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
