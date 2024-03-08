using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hackathon.Migrations
{
    /// <inheritdoc />
    public partial class AddAasId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemoteDiscoveryUrl",
                table: "ImportedShells",
                newName: "AasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AasId",
                table: "ImportedShells",
                newName: "RemoteDiscoveryUrl");
        }
    }
}
