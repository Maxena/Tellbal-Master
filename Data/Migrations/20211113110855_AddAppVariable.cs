using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class AddAppVariable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppVariables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutUs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityAndPrivacy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppVariables", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppVariables",
                columns: new[] { "Id", "AboutUs", "SecurityAndPrivacy", "TermsAndConditions" },
                values: new object[] { 1, "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ", "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ", "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppVariables");
        }
    }
}
