using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TDD.Migrations
{
    /// <inheritdoc />
    public partial class initM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__EFMigrationsHistory",
                columns: table => new
                {
                    MigrationId = table.Column<string>(type: "TEXT", nullable: false),
                    ProductVersion = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___EFMigrationsHistory", x => x.MigrationId);
                });

            migrationBuilder.CreateTable(
                name: "Adherent",
                columns: table => new
                {
                    code = table.Column<string>(type: "TEXT", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Civilite = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adherent", x => x.code);
                });

            migrationBuilder.CreateTable(
                name: "Livres",
                columns: table => new
                {
                    isbn = table.Column<string>(type: "TEXT", nullable: false),
                    Titre = table.Column<string>(type: "TEXT", nullable: false),
                    Auteur = table.Column<string>(type: "TEXT", nullable: false),
                    Editeur = table.Column<string>(type: "TEXT", nullable: false),
                    Format = table.Column<int>(type: "INTEGER", nullable: false),
                    Disponible = table.Column<bool>(type: "INTEGER", nullable: false),
                    AdherentCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livres", x => x.isbn);
                    table.ForeignKey(
                        name: "FK_Livres_Adherent_AdherentCode",
                        column: x => x.AdherentCode,
                        principalTable: "Adherent",
                        principalColumn: "code");
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    isbn = table.Column<string>(type: "TEXT", nullable: false),
                    LivreIsbn = table.Column<string>(type: "TEXT", nullable: true),
                    dateDebut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AdherentCode = table.Column<string>(type: "TEXT", nullable: false),
                    dateFin = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Reservations_Adherent_AdherentCode",
                        column: x => x.AdherentCode,
                        principalTable: "Adherent",
                        principalColumn: "code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Livres_LivreIsbn",
                        column: x => x.LivreIsbn,
                        principalTable: "Livres",
                        principalColumn: "isbn");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livres_AdherentCode",
                table: "Livres",
                column: "AdherentCode");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_AdherentCode",
                table: "Reservations",
                column: "AdherentCode");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_LivreIsbn",
                table: "Reservations",
                column: "LivreIsbn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__EFMigrationsHistory");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Livres");

            migrationBuilder.DropTable(
                name: "Adherent");
        }
    }
}
