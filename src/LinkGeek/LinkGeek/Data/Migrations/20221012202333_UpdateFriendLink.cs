using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkGeek.Data.Migrations
{
    public partial class UpdateFriendLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkFriend_AspNetUsers_User1Id",
                table: "FriendLinkFriend");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkFriend_AspNetUsers_User2Id",
                table: "FriendLinkFriend");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkIncoming_AspNetUsers_User1Id",
                table: "FriendLinkIncoming");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkIncoming_AspNetUsers_User2Id",
                table: "FriendLinkIncoming");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkOutgoing_AspNetUsers_User1Id",
                table: "FriendLinkOutgoing");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkOutgoing_AspNetUsers_User2Id",
                table: "FriendLinkOutgoing");

            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "FriendLinkOutgoing",
                newName: "ToId");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                table: "FriendLinkOutgoing",
                newName: "FromId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkOutgoing_User2Id",
                table: "FriendLinkOutgoing",
                newName: "IX_FriendLinkOutgoing_ToId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkOutgoing_User1Id",
                table: "FriendLinkOutgoing",
                newName: "IX_FriendLinkOutgoing_FromId");

            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "FriendLinkIncoming",
                newName: "ToId");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                table: "FriendLinkIncoming",
                newName: "FromId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkIncoming_User2Id",
                table: "FriendLinkIncoming",
                newName: "IX_FriendLinkIncoming_ToId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkIncoming_User1Id",
                table: "FriendLinkIncoming",
                newName: "IX_FriendLinkIncoming_FromId");

            migrationBuilder.RenameColumn(
                name: "User2Id",
                table: "FriendLinkFriend",
                newName: "ToId");

            migrationBuilder.RenameColumn(
                name: "User1Id",
                table: "FriendLinkFriend",
                newName: "FromId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkFriend_User2Id",
                table: "FriendLinkFriend",
                newName: "IX_FriendLinkFriend_ToId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkFriend_User1Id",
                table: "FriendLinkFriend",
                newName: "IX_FriendLinkFriend_FromId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkFriend_AspNetUsers_FromId",
                table: "FriendLinkFriend",
                column: "FromId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkFriend_AspNetUsers_ToId",
                table: "FriendLinkFriend",
                column: "ToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkIncoming_AspNetUsers_FromId",
                table: "FriendLinkIncoming",
                column: "FromId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkIncoming_AspNetUsers_ToId",
                table: "FriendLinkIncoming",
                column: "ToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkOutgoing_AspNetUsers_FromId",
                table: "FriendLinkOutgoing",
                column: "FromId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkOutgoing_AspNetUsers_ToId",
                table: "FriendLinkOutgoing",
                column: "ToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkFriend_AspNetUsers_FromId",
                table: "FriendLinkFriend");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkFriend_AspNetUsers_ToId",
                table: "FriendLinkFriend");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkIncoming_AspNetUsers_FromId",
                table: "FriendLinkIncoming");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkIncoming_AspNetUsers_ToId",
                table: "FriendLinkIncoming");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkOutgoing_AspNetUsers_FromId",
                table: "FriendLinkOutgoing");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendLinkOutgoing_AspNetUsers_ToId",
                table: "FriendLinkOutgoing");

            migrationBuilder.RenameColumn(
                name: "ToId",
                table: "FriendLinkOutgoing",
                newName: "User2Id");

            migrationBuilder.RenameColumn(
                name: "FromId",
                table: "FriendLinkOutgoing",
                newName: "User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkOutgoing_ToId",
                table: "FriendLinkOutgoing",
                newName: "IX_FriendLinkOutgoing_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkOutgoing_FromId",
                table: "FriendLinkOutgoing",
                newName: "IX_FriendLinkOutgoing_User1Id");

            migrationBuilder.RenameColumn(
                name: "ToId",
                table: "FriendLinkIncoming",
                newName: "User2Id");

            migrationBuilder.RenameColumn(
                name: "FromId",
                table: "FriendLinkIncoming",
                newName: "User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkIncoming_ToId",
                table: "FriendLinkIncoming",
                newName: "IX_FriendLinkIncoming_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkIncoming_FromId",
                table: "FriendLinkIncoming",
                newName: "IX_FriendLinkIncoming_User1Id");

            migrationBuilder.RenameColumn(
                name: "ToId",
                table: "FriendLinkFriend",
                newName: "User2Id");

            migrationBuilder.RenameColumn(
                name: "FromId",
                table: "FriendLinkFriend",
                newName: "User1Id");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkFriend_ToId",
                table: "FriendLinkFriend",
                newName: "IX_FriendLinkFriend_User2Id");

            migrationBuilder.RenameIndex(
                name: "IX_FriendLinkFriend_FromId",
                table: "FriendLinkFriend",
                newName: "IX_FriendLinkFriend_User1Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkFriend_AspNetUsers_User1Id",
                table: "FriendLinkFriend",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkFriend_AspNetUsers_User2Id",
                table: "FriendLinkFriend",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkIncoming_AspNetUsers_User1Id",
                table: "FriendLinkIncoming",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkIncoming_AspNetUsers_User2Id",
                table: "FriendLinkIncoming",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkOutgoing_AspNetUsers_User1Id",
                table: "FriendLinkOutgoing",
                column: "User1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendLinkOutgoing_AspNetUsers_User2Id",
                table: "FriendLinkOutgoing",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
