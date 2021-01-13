using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parkner.Api.Migrations
{
    public partial class Primera : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creacion = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                },
                comment: "Consumidores del servicio");

            migrationBuilder.CreateTable(
                name: "Empleados",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creacion = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                },
                comment: "Recepcionistas del servicio/Trabajadores de los Estacionamientos");

            migrationBuilder.CreateTable(
                name: "Responsables",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creacion = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responsables", x => x.Id);
                },
                comment: "Proveedores del servicio/Responsables de Estacionamientos y de sus Empleados");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creacion = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                },
                comment: "Administradores del servicio");

            migrationBuilder.CreateTable(
                name: "Estacionamientos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Costo = table.Column<decimal>(type: "Decimal(10,2)", nullable: false),
                    Foto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calificacion = table.Column<decimal>(type: "Decimal(5,2)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Creacion = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    Concurrido = table.Column<bool>(type: "bit", nullable: false),
                    ResponsableId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estacionamientos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responsable_Estacionamientos",
                        column: x => x.ResponsableId,
                        principalTable: "Responsables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Activos del proveedor del servicio");

            migrationBuilder.CreateTable(
                name: "Ganancias",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Cantidad = table.Column<decimal>(type: "Decimal(10,2)", nullable: false),
                    ResponsableId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ganancias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responsable_Ganancias",
                        column: x => x.ResponsableId,
                        principalTable: "Responsables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Beneficios para el prooveedor del servicio por la renta de sus activos");

            migrationBuilder.CreateTable(
                name: "Cajones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    EstacionamientoId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cajones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estacionamiento_Cajones",
                        column: x => x.EstacionamientoId,
                        principalTable: "Estacionamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Producto ofertado para el consumo del Cliente");

            migrationBuilder.CreateTable(
                name: "Direcciones",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntreCalles = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Colonia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Municipio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitud = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitud = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstacionamientoId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Direcciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Direccion_Estacionamiento",
                        column: x => x.EstacionamientoId,
                        principalTable: "Estacionamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auxiliar de Estacionamientos");

            migrationBuilder.CreateTable(
                name: "EstacionamientosEmpleados",
                columns: table => new
                {
                    EstacionamientoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmpleadoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstacionamientosEmpleados", x => new { x.EmpleadoId, x.EstacionamientoId });
                    table.ForeignKey(
                        name: "FK_EstacionamientosEmpleados_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EstacionamientosEmpleados_Estacionamientos_EstacionamientoId",
                        column: x => x.EstacionamientoId,
                        principalTable: "Estacionamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Empleados del Estacionamientos/Estacionamientos del empleado");

            migrationBuilder.CreateTable(
                name: "Horarios",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Dia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaNumero = table.Column<int>(type: "int", nullable: false),
                    Inicio = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Fin = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    EstacionamientoId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estacionamiento_Horarios",
                        column: x => x.EstacionamientoId,
                        principalTable: "Estacionamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Auxiliar de Estacionamientos");

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Tolerancia = table.Column<int>(type: "int", nullable: false),
                    Inicio = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Fin = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    Cobrado = table.Column<decimal>(type: "Decimal(10,2)", nullable: true),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    ClienteId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EstacionamientoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmpleadoInicializadorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmpleadoFinalizadorId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cliente_Reservas",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpleadoFin_Reservas",
                        column: x => x.EmpleadoFinalizadorId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmpleadoInicio_Reservas",
                        column: x => x.EmpleadoInicializadorId,
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Estacionamiento_Reservas",
                        column: x => x.EstacionamientoId,
                        principalTable: "Estacionamientos",
                        principalColumn: "Id");
                },
                comment: "Consumo de Cajones en un Estacionamiento por los Clientes");

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Apellido", "Clave", "Correo", "Creacion", "Disponible", "Foto", "Nombre", "Rol" },
                values: new object[,]
                {
                    { "3a0e3396-3557-4fe6-8532-bf72af2c8c95", "A", "OGgFiKDvH7RfNOasdZLhfw==", "a@c.co", new DateTime(2020, 12, 15, 21, 6, 30, 129, DateTimeKind.Local).AddTicks(7183), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Cliente", "Cliente" },
                    { "5df10ce3-15ca-4164-abe0-20eea0393611", "B", "OGgFiKDvH7RfNOasdZLhfw==", "b@c.co", new DateTime(2020, 12, 15, 21, 6, 30, 129, DateTimeKind.Local).AddTicks(8064), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Cliente", "Cliente" },
                    { "3b384e16-6076-496a-bc20-5f549e9543c7", "C", "OGgFiKDvH7RfNOasdZLhfw==", "c@c.co", new DateTime(2020, 12, 15, 21, 6, 30, 129, DateTimeKind.Local).AddTicks(8078), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Cliente", "Cliente" },
                    { "eb5f0df7-23e3-413a-809d-b246b649a1c7", "D", "OGgFiKDvH7RfNOasdZLhfw==", "d@c.co", new DateTime(2020, 12, 15, 21, 6, 30, 129, DateTimeKind.Local).AddTicks(8081), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Cliente", "Cliente" },
                    { "38f3d3a8-5b77-4e7f-9744-2e4dfbf75a93", "E", "OGgFiKDvH7RfNOasdZLhfw==", "e@c.co", new DateTime(2020, 12, 15, 21, 6, 30, 129, DateTimeKind.Local).AddTicks(8084), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Cliente", "Cliente" }
                });

            migrationBuilder.InsertData(
                table: "Empleados",
                columns: new[] { "Id", "Apellido", "Clave", "Correo", "Creacion", "Disponible", "Foto", "Nombre", "Rol" },
                values: new object[,]
                {
                    { "38f3d3a8-5b77-4e7f-9744-2e4dfbf75a93", "E", "OGgFiKDvH7RfNOasdZLhfw==", "e@e.co", new DateTime(2020, 12, 15, 21, 6, 30, 127, DateTimeKind.Local).AddTicks(3197), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Empleado", "Empleado" },
                    { "3b384e16-6076-496a-bc20-5f549e9543c7", "C", "OGgFiKDvH7RfNOasdZLhfw==", "c@e.co", new DateTime(2020, 12, 15, 21, 6, 30, 127, DateTimeKind.Local).AddTicks(3190), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Empleado", "Empleado" },
                    { "eb5f0df7-23e3-413a-809d-b246b649a1c7", "D", "OGgFiKDvH7RfNOasdZLhfw==", "d@e.co", new DateTime(2020, 12, 15, 21, 6, 30, 127, DateTimeKind.Local).AddTicks(3194), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Empleado", "Empleado" },
                    { "3a0e3396-3557-4fe6-8532-bf72af2c8c95", "A", "OGgFiKDvH7RfNOasdZLhfw==", "a@e.co", new DateTime(2020, 12, 15, 21, 6, 30, 127, DateTimeKind.Local).AddTicks(2115), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Empleado", "Empleado" },
                    { "5df10ce3-15ca-4164-abe0-20eea0393611", "B", "OGgFiKDvH7RfNOasdZLhfw==", "b@e.co", new DateTime(2020, 12, 15, 21, 6, 30, 127, DateTimeKind.Local).AddTicks(3174), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Empleado", "Empleado" }
                });

            migrationBuilder.InsertData(
                table: "Responsables",
                columns: new[] { "Id", "Apellido", "Clave", "Correo", "Creacion", "Disponible", "Foto", "Nombre", "Rol" },
                values: new object[,]
                {
                    { "243729f5-1863-4335-9cf8-51b41320321a", "F", "OGgFiKDvH7RfNOasdZLhfw==", "f@r.co", new DateTime(2020, 12, 15, 21, 6, 30, 128, DateTimeKind.Local).AddTicks(9377), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Responsable", "Responsable" },
                    { "ee9646ef-62c7-4649-b1e4-2ffe8c71abf4", "A", "OGgFiKDvH7RfNOasdZLhfw==", "a@r.co", new DateTime(2020, 12, 15, 21, 6, 30, 128, DateTimeKind.Local).AddTicks(8369), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Responsable", "Responsable" },
                    { "a9cd9063-4668-452c-807d-547aacbf1da7", "B", "OGgFiKDvH7RfNOasdZLhfw==", "b@r.co", new DateTime(2020, 12, 15, 21, 6, 30, 128, DateTimeKind.Local).AddTicks(9344), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Responsable", "Responsable" },
                    { "504643bc-7c6c-43a5-bd83-2c82fa464981", "C", "OGgFiKDvH7RfNOasdZLhfw==", "c@r.co", new DateTime(2020, 12, 15, 21, 6, 30, 128, DateTimeKind.Local).AddTicks(9359), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Responsable", "Responsable" },
                    { "95d60d39-6f6d-499c-9e21-77e7d6bb7040", "D", "OGgFiKDvH7RfNOasdZLhfw==", "d@r.co", new DateTime(2020, 12, 15, 21, 6, 30, 128, DateTimeKind.Local).AddTicks(9363), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Responsable", "Responsable" },
                    { "049613e9-4b0a-4c69-bbda-f5d14f1d6fc3", "E", "OGgFiKDvH7RfNOasdZLhfw==", "e@r.co", new DateTime(2020, 12, 15, 21, 6, 30, 128, DateTimeKind.Local).AddTicks(9365), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Responsable", "Responsable" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Apellido", "Clave", "Correo", "Creacion", "Disponible", "Foto", "Nombre", "Rol" },
                values: new object[] { "21a173bb-3116-4f35-a95f-35b6be172bfb", "General", "OGgFiKDvH7RfNOasdZLhfw==", "a@p.co", new DateTime(2020, 12, 15, 21, 6, 30, 83, DateTimeKind.Local).AddTicks(8485), true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "Administrador", "Administrador" });

            migrationBuilder.InsertData(
                table: "Estacionamientos",
                columns: new[] { "Id", "Calificacion", "Concurrido", "Costo", "Creacion", "Descripcion", "Disponible", "Foto", "Nombre", "ResponsableId", "Tipo" },
                values: new object[] { "6478d74c-68e0-4e91-98a9-024040e302e2", null, false, 5m, new DateTime(2020, 12, 15, 21, 6, 30, 128, DateTimeKind.Local).AddTicks(3811), "Estacionamiento en terreno particular, junto a la plaza principal", true, "images/avatares/empleados/08b874f3-481e-44e9-9286-caeabb775860.png", "San Pancho", "ee9646ef-62c7-4649-b1e4-2ffe8c71abf4", "Terreno" });

            migrationBuilder.InsertData(
                table: "Cajones",
                columns: new[] { "Id", "Disponible", "EstacionamientoId", "Nombre" },
                values: new object[,]
                {
                    { "331e78ae-2506-466f-ac8d-ff53d18e3e0c", true, "6478d74c-68e0-4e91-98a9-024040e302e2", "A" },
                    { "4300d849-6fc0-4826-91d5-2822adcab7ad", true, "6478d74c-68e0-4e91-98a9-024040e302e2", "B" },
                    { "2b1efc5c-9331-4229-8831-7a76ae73bbf5", true, "6478d74c-68e0-4e91-98a9-024040e302e2", "C" },
                    { "b8483061-dc60-4586-85e8-58b01b4bed62", true, "6478d74c-68e0-4e91-98a9-024040e302e2", "D" },
                    { "aaee52f6-b966-45e0-bc96-787777be8adf", true, "6478d74c-68e0-4e91-98a9-024040e302e2", "E" }
                });

            migrationBuilder.InsertData(
                table: "Direcciones",
                columns: new[] { "Id", "Calle", "CodigoPostal", "Colonia", "EntreCalles", "EstacionamientoId", "Latitud", "Longitud", "Municipio", "Numero" },
                values: new object[] { "ad222b8b-ea21-4fe7-ac01-d3e9f7f325fe", "San Cuilmas", "28001", "San Pancho", "San Pedro y San Pablo", "6478d74c-68e0-4e91-98a9-024040e302e2", "19.239777", "-103.729653", "Colima", "1A" });

            migrationBuilder.InsertData(
                table: "EstacionamientosEmpleados",
                columns: new[] { "EmpleadoId", "EstacionamientoId" },
                values: new object[,]
                {
                    { "38f3d3a8-5b77-4e7f-9744-2e4dfbf75a93", "6478d74c-68e0-4e91-98a9-024040e302e2" },
                    { "eb5f0df7-23e3-413a-809d-b246b649a1c7", "6478d74c-68e0-4e91-98a9-024040e302e2" },
                    { "3b384e16-6076-496a-bc20-5f549e9543c7", "6478d74c-68e0-4e91-98a9-024040e302e2" },
                    { "5df10ce3-15ca-4164-abe0-20eea0393611", "6478d74c-68e0-4e91-98a9-024040e302e2" },
                    { "3a0e3396-3557-4fe6-8532-bf72af2c8c95", "6478d74c-68e0-4e91-98a9-024040e302e2" }
                });

            migrationBuilder.InsertData(
                table: "Horarios",
                columns: new[] { "Id", "Dia", "DiaNumero", "EstacionamientoId", "Fin", "Inicio" },
                values: new object[,]
                {
                    { "b0718cf7-1c7f-44d1-887e-79d64de33dcc", "Lunes", 1, "6478d74c-68e0-4e91-98a9-024040e302e2", new DateTime(2020, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "cd1fb926-b2ea-45ad-a3a2-2ad1142f8e9a", "Martes", 2, "6478d74c-68e0-4e91-98a9-024040e302e2", new DateTime(2020, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "dd83add4-bbe7-48e0-ab41-21759d940b54", "Miércoles", 3, "6478d74c-68e0-4e91-98a9-024040e302e2", new DateTime(2020, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "bfc61cc9-3796-4d36-922a-3afdef7d9818", "Jueves", 4, "6478d74c-68e0-4e91-98a9-024040e302e2", new DateTime(2020, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "f264ca4f-f127-41a9-b164-9425741ebdfe", "Viernes", 5, "6478d74c-68e0-4e91-98a9-024040e302e2", new DateTime(2020, 1, 1, 19, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 1, 7, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "4db8abfa-920d-41b8-b973-24c987c55338", "Sábados", 6, "6478d74c-68e0-4e91-98a9-024040e302e2", new DateTime(2020, 1, 1, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cajones_EstacionamientoId",
                table: "Cajones",
                column: "EstacionamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Direcciones_EstacionamientoId",
                table: "Direcciones",
                column: "EstacionamientoId",
                unique: true,
                filter: "[EstacionamientoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Estacionamientos_ResponsableId",
                table: "Estacionamientos",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_EstacionamientosEmpleados_EstacionamientoId",
                table: "EstacionamientosEmpleados",
                column: "EstacionamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ganancias_ResponsableId",
                table: "Ganancias",
                column: "ResponsableId");

            migrationBuilder.CreateIndex(
                name: "IX_Horarios_EstacionamientoId",
                table: "Horarios",
                column: "EstacionamientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ClienteId",
                table: "Reservas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_EmpleadoFinalizadorId",
                table: "Reservas",
                column: "EmpleadoFinalizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_EmpleadoInicializadorId",
                table: "Reservas",
                column: "EmpleadoInicializadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_EstacionamientoId",
                table: "Reservas",
                column: "EstacionamientoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cajones");

            migrationBuilder.DropTable(
                name: "Direcciones");

            migrationBuilder.DropTable(
                name: "EstacionamientosEmpleados");

            migrationBuilder.DropTable(
                name: "Ganancias");

            migrationBuilder.DropTable(
                name: "Horarios");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Empleados");

            migrationBuilder.DropTable(
                name: "Estacionamientos");

            migrationBuilder.DropTable(
                name: "Responsables");
        }
    }
}
