using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EfficientDesigner_Service.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataSources",
                columns: table => new
                {
                    DataSourceId = table.Column<Guid>(nullable: false),
                    Api = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSources", x => x.DataSourceId);
                });

            migrationBuilder.CreateTable(
                name: "Layouts",
                columns: table => new
                {
                    LayoutId = table.Column<Guid>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    File = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layouts", x => x.LayoutId);
                });

            migrationBuilder.CreateTable(
                name: "PropertyBindings",
                columns: table => new
                {
                    PropertyBindingId = table.Column<Guid>(nullable: false),
                    PropertyName = table.Column<string>(nullable: true),
                    ElementName = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    LayoutId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyBindings", x => x.PropertyBindingId);
                    table.ForeignKey(
                        name: "FK_PropertyBindings_Layouts_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "Layouts",
                        principalColumn: "LayoutId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyBindings_LayoutId",
                table: "PropertyBindings",
                column: "LayoutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataSources");

            migrationBuilder.DropTable(
                name: "PropertyBindings");

            migrationBuilder.DropTable(
                name: "Layouts");
        }
    }
}
