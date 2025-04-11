using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TP1Datos.Migrations
{
    /// <inheritdoc />
    public partial class InitialCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Clientes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
            //        Dni = table.Column<int>(type: "int", maxLength: 100, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Clientes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Ordenes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        NumeroOrden = table.Column<int>(type: "int", maxLength: 100, nullable: false),
            //        FechaOrden = table.Column<DateOnly>(type: "Date", nullable: false),
            //        Valor = table.Column<double>(type: "float", maxLength: 100, nullable: false),
            //        ClienteId = table.Column<int>(type: "int", maxLength: 100, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Ordenes", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Ordenes_Clientes_ClienteId",
            //            column: x => x.ClienteId,
            //            principalTable: "Clientes",
            //            principalColumn: "Id");
            //    });

            //migrationBuilder.InsertData(
            //    table: "Clientes",
            //    columns: new[] { "Id", "Apellido", "Dni", "Nombre" },
            //    values: new object[] { 1, "Marhon", 40377284, "Daniel" });

            //migrationBuilder.InsertData(
            //    table: "Ordenes",
            //    columns: new[] { "Id", "ClienteId", "FechaOrden", "NumeroOrden", "Valor" },
            //    values: new object[,]
            //    {
            //        { 1, 1, new DateOnly(1986, 10, 30), 1, 0.0 },
            //        { 2, 1, new DateOnly(1953, 10, 10), 2, 0.0 }
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "Clientes_Nombre_Apellido",
            //    table: "Clientes",
            //    columns: new[] { "Nombre", "Apellido" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Ordenes_ClienteId",
            //    table: "Ordenes",
            //    column: "ClienteId");

            //migrationBuilder.CreateIndex(
            //    name: "Orden_Numero_ClienteId",
            //    table: "Ordenes",
            //    columns: new[] { "NumeroOrden", "ClienteId" },
            //    unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
