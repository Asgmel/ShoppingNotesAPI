using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingNotes.Migrations
{
    /// <inheritdoc />
    public partial class NotesUpdatedRelationsships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListId",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Notes",
                newName: "SListId");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Lists",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_SListId",
                table: "Notes",
                column: "SListId");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_UserId",
                table: "Lists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lists_Users_UserId",
                table: "Lists",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Lists_SListId",
                table: "Notes",
                column: "SListId",
                principalTable: "Lists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lists_Users_UserId",
                table: "Lists");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Lists_SListId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_SListId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Lists_UserId",
                table: "Lists");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Lists");

            migrationBuilder.RenameColumn(
                name: "SListId",
                table: "Notes",
                newName: "OwnerId");

            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "Notes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
