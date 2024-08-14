using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewPetHome.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    experience = table.Column<int>(type: "integer", nullable: false),
                    count_pets_find_home = table.Column<int>(type: "integer", nullable: false),
                    count_pets_looking_home = table.Column<int>(type: "integer", nullable: false),
                    count_pets_in_treatment = table.Column<int>(type: "integer", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    species = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    breed = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    health_info = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    weight = table.Column<double>(type: "double precision", nullable: false),
                    height = table.Column<double>(type: "double precision", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_castrated = table.Column<bool>(type: "boolean", nullable: false),
                    birth_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "social_network",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_social_network", x => x.id);
                    table.ForeignKey(
                        name: "fk_social_network_volunteers_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "pet_photo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    path = table.Column<string>(type: "text", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pet_photo", x => x.id);
                    table.ForeignKey(
                        name: "fk_pet_photo_pets_pet_id",
                        column: x => x.pet_id,
                        principalTable: "pets",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "requisites",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: true),
                    volunteer_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_requisites", x => x.id);
                    table.ForeignKey(
                        name: "fk_requisites_pet_pet_id",
                        column: x => x.pet_id,
                        principalTable: "pets",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_requisites_volunteer_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_pet_photo_pet_id",
                table: "pet_photo",
                column: "pet_id");

            migrationBuilder.CreateIndex(
                name: "ix_pets_volunteer_id",
                table: "pets",
                column: "volunteer_id");

            migrationBuilder.CreateIndex(
                name: "ix_requisites_pet_id",
                table: "requisites",
                column: "pet_id");

            migrationBuilder.CreateIndex(
                name: "ix_requisites_volunteer_id",
                table: "requisites",
                column: "volunteer_id");

            migrationBuilder.CreateIndex(
                name: "ix_social_network_volunteer_id",
                table: "social_network",
                column: "volunteer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pet_photo");

            migrationBuilder.DropTable(
                name: "requisites");

            migrationBuilder.DropTable(
                name: "social_network");

            migrationBuilder.DropTable(
                name: "pets");

            migrationBuilder.DropTable(
                name: "volunteers");
        }
    }
}
