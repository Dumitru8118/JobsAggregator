using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobHub.API.Migrations
{
    /// <inheritdoc />
    public partial class ColumnChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DatePosted",
                table: "Jobs",
                newName: "Date Posted");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Jobs",
                newName: "JobPageId");

            migrationBuilder.CreateTable(
                name: "JobPages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Salary = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    JobType = table.Column<string>(type: "text", nullable: false),
                    ExpertiseLvl = table.Column<string>(type: "text", nullable: false),
                    Industry = table.Column<string>(type: "text", nullable: false),
                    QualificationLvl = table.Column<string>(type: "text", nullable: false),
                    Department = table.Column<string>(type: "text", nullable: false),
                    Language = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_JobPageId",
                table: "Jobs",
                column: "JobPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_JobPages_JobPageId",
                table: "Jobs",
                column: "JobPageId",
                principalTable: "JobPages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_JobPages_JobPageId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "JobPages");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_JobPageId",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "Date Posted",
                table: "Jobs",
                newName: "DatePosted");

            migrationBuilder.RenameColumn(
                name: "JobPageId",
                table: "Jobs",
                newName: "Description");
        }
    }
}
