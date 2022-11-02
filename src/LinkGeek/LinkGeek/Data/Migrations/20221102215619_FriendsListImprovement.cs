using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkGeek.Data.Migrations
{
    public partial class FriendsListImprovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_MyRealFriendsId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_RealFriendsId",
                table: "Friends");

            migrationBuilder.DropTable(
                name: "FriendLinkFriend");

            migrationBuilder.DropTable(
                name: "FriendLinkIncoming");

            migrationBuilder.DropTable(
                name: "FriendLinkOutgoing");

            migrationBuilder.RenameColumn(
                name: "RealFriendsId",
                table: "Friends",
                newName: "MyFriendsId");

            migrationBuilder.RenameColumn(
                name: "MyRealFriendsId",
                table: "Friends",
                newName: "FriendsId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_RealFriendsId",
                table: "Friends",
                newName: "IX_Friends_MyFriendsId");

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    ReceivedFriendRequestsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SentFriendRequestsId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => new { x.ReceivedFriendRequestsId, x.SentFriendRequestsId });
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_ReceivedFriendRequestsId",
                        column: x => x.ReceivedFriendRequestsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_SentFriendRequestsId",
                        column: x => x.SentFriendRequestsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_SentFriendRequestsId",
                table: "FriendRequests",
                column: "SentFriendRequestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_FriendsId",
                table: "Friends",
                column: "FriendsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_MyFriendsId",
                table: "Friends",
                column: "MyFriendsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_FriendsId",
                table: "Friends");

            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_MyFriendsId",
                table: "Friends");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.RenameColumn(
                name: "MyFriendsId",
                table: "Friends",
                newName: "RealFriendsId");

            migrationBuilder.RenameColumn(
                name: "FriendsId",
                table: "Friends",
                newName: "MyRealFriendsId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_MyFriendsId",
                table: "Friends",
                newName: "IX_Friends_RealFriendsId");

            migrationBuilder.CreateTable(
                name: "FriendLinkFriend",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLinkFriend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendLinkFriend_AspNetUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendLinkFriend_AspNetUsers_ToId",
                        column: x => x.ToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FriendLinkIncoming",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLinkIncoming", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendLinkIncoming_AspNetUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FriendLinkIncoming_AspNetUsers_ToId",
                        column: x => x.ToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendLinkOutgoing",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FromId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ToId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLinkOutgoing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendLinkOutgoing_AspNetUsers_FromId",
                        column: x => x.FromId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendLinkOutgoing_AspNetUsers_ToId",
                        column: x => x.ToId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkFriend_FromId",
                table: "FriendLinkFriend",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkFriend_ToId",
                table: "FriendLinkFriend",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkIncoming_FromId",
                table: "FriendLinkIncoming",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkIncoming_ToId",
                table: "FriendLinkIncoming",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkOutgoing_FromId",
                table: "FriendLinkOutgoing",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkOutgoing_ToId",
                table: "FriendLinkOutgoing",
                column: "ToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_MyRealFriendsId",
                table: "Friends",
                column: "MyRealFriendsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_RealFriendsId",
                table: "Friends",
                column: "RealFriendsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
