using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkGeek.Data.Migrations
{
    public partial class AddFriendLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendLinkFriend",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User1Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User2Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLinkFriend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendLinkFriend_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FriendLinkFriend_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FriendLinkIncoming",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User1Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User2Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLinkIncoming", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendLinkIncoming_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FriendLinkIncoming_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FriendLinkOutgoing",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User1Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User2Id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLinkOutgoing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendLinkOutgoing_AspNetUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FriendLinkOutgoing_AspNetUsers_User2Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkFriend_User1Id",
                table: "FriendLinkFriend",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkFriend_User2Id",
                table: "FriendLinkFriend",
                column: "User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkIncoming_User1Id",
                table: "FriendLinkIncoming",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkIncoming_User2Id",
                table: "FriendLinkIncoming",
                column: "User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkOutgoing_User1Id",
                table: "FriendLinkOutgoing",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendLinkOutgoing_User2Id",
                table: "FriendLinkOutgoing",
                column: "User2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendLinkFriend");

            migrationBuilder.DropTable(
                name: "FriendLinkIncoming");

            migrationBuilder.DropTable(
                name: "FriendLinkOutgoing");
        }
    }
}
