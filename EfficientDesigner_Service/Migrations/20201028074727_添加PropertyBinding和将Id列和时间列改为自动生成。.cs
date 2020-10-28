using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EfficientDesigner_Service.Migrations
{
    public partial class 添加PropertyBinding和将Id列和时间列改为自动生成 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PropertyBinding",
                columns: table => new
                {
                    PropertyBindingId = table.Column<Guid>(nullable: false),
                    PropertyName = table.Column<string>(nullable: true),
                    ElementName = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    LayoutId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyBinding", x => x.PropertyBindingId);
                    table.ForeignKey(
                        name: "FK_PropertyBinding_Layouts_LayoutId",
                        column: x => x.LayoutId,
                        principalTable: "Layouts",
                        principalColumn: "LayoutId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyBinding_LayoutId",
                table: "PropertyBinding",
                column: "LayoutId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyBinding");
        }
    }
}
