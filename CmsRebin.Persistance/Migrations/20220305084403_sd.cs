using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsRebin.Persistance.Migrations
{
    public partial class sd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FieldsofTable",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdTable = table.Column<long>(type: "bigint", nullable: false),
                    fieldname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    relation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    interfaces = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldsofTable", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PermitionstoActivitie",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitionstoActivitie", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RelationsofTable",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    one_collection = table.Column<long>(type: "bigint", nullable: false),
                    one_field = table.Column<long>(type: "bigint", nullable: false),
                    many_collection = table.Column<long>(type: "bigint", nullable: false),
                    many_field = table.Column<long>(type: "bigint", nullable: false),
                    TypeofReleation = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelationsofTable", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    rolename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    singleton = table.Column<bool>(type: "bit", nullable: false),
                    IdDBase = table.Column<long>(type: "bigint", nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TypeofField",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typefield = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeofField", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TypeofReleation",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typerelation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeofReleation", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Roleid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                    table.ForeignKey(
                        name: "FK_Users_Role_Roleid",
                        column: x => x.Roleid,
                        principalTable: "Role",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "DatabaseLists",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DBName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Userid = table.Column<long>(type: "bigint", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseLists", x => x.id);
                    table.ForeignKey(
                        name: "FK_DatabaseLists_Users_Userid",
                        column: x => x.Userid,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ModelContainer",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fieldsofTablesid = table.Column<long>(type: "bigint", nullable: true),
                    relationsofTablesid = table.Column<long>(type: "bigint", nullable: true),
                    tablesid = table.Column<long>(type: "bigint", nullable: true),
                    typeofReleationid = table.Column<long>(type: "bigint", nullable: true),
                    typeofFieldid = table.Column<long>(type: "bigint", nullable: true),
                    permitionstoActivitiesid = table.Column<long>(type: "bigint", nullable: true),
                    roleid = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    userid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelContainer", x => x.id);
                    table.ForeignKey(
                        name: "FK_ModelContainer_FieldsofTable_fieldsofTablesid",
                        column: x => x.fieldsofTablesid,
                        principalTable: "FieldsofTable",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ModelContainer_PermitionstoActivitie_permitionstoActivitiesid",
                        column: x => x.permitionstoActivitiesid,
                        principalTable: "PermitionstoActivitie",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ModelContainer_RelationsofTable_relationsofTablesid",
                        column: x => x.relationsofTablesid,
                        principalTable: "RelationsofTable",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ModelContainer_Role_roleid",
                        column: x => x.roleid,
                        principalTable: "Role",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ModelContainer_Tables_tablesid",
                        column: x => x.tablesid,
                        principalTable: "Tables",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ModelContainer_TypeofField_typeofFieldid",
                        column: x => x.typeofFieldid,
                        principalTable: "TypeofField",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ModelContainer_TypeofReleation_typeofReleationid",
                        column: x => x.typeofReleationid,
                        principalTable: "TypeofReleation",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ModelContainer_Users_userid",
                        column: x => x.userid,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tokens_Users_id",
                        column: x => x.id,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "id", "InsertTime", "IsRemoved", "RemoveTime", "UpdateTime", "rolename" },
                values: new object[,]
                {
                    { "AQAAAAEAACcQAAAAEB4wNoDqGyACrBZ2v8P9cUmTa+ck14dPufX+J58eLpEmpPukxjpyrT2N9xzKL5E8JQ==", new DateTime(2022, 3, 5, 12, 14, 3, 22, DateTimeKind.Local).AddTicks(5105), false, null, null, "admin" },
                    { "AQAAAAEAACcQAAAAEGCtAmEFvmovRZ1NwQ+u3zwVEu+hIA7hRKv/OkznkTVl8OS43ttdkR0JIYuc4xssIg==", new DateTime(2022, 3, 5, 12, 14, 3, 29, DateTimeKind.Local).AddTicks(544), false, null, null, "operator" },
                    { "AQAAAAEAACcQAAAAEMic1aivu3144bJb/7r3zy/E03bicQ15rQqPN7yG8bwTjmqR8K78aNATYPA0sJ2iUA==", new DateTime(2022, 3, 5, 12, 14, 3, 35, DateTimeKind.Local).AddTicks(7110), false, null, null, "user" }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "id", "IdDBase", "InsertTime", "IsRemoved", "RemoveTime", "UpdateTime", "collection", "note", "singleton" },
                values: new object[,]
                {
                    { 1L, 0L, new DateTime(2022, 3, 5, 12, 14, 3, 22, DateTimeKind.Local).AddTicks(4821), false, null, null, "Role", "", false },
                    { 2L, 0L, new DateTime(2022, 3, 5, 12, 14, 3, 22, DateTimeKind.Local).AddTicks(4869), false, null, null, "Users", "", false }
                });

            migrationBuilder.InsertData(
                table: "TypeofField",
                columns: new[] { "id", "typefield" },
                values: new object[,]
                {
                    { 1L, "int" },
                    { 2L, "bigint" },
                    { 3L, "nvarchar(50)" },
                    { 4L, "binary(50)" },
                    { 5L, "text" },
                    { 6L, "bit" },
                    { 7L, "char(10)" },
                    { 8L, "date" },
                    { 9L, "datetime" },
                    { 10L, "datetime2(7)" },
                    { 11L, "datetimeoffset(7)" },
                    { 12L, "decimal(18,0)" },
                    { 13L, "float" },
                    { 14L, "geography" },
                    { 15L, "geometry" },
                    { 16L, "hierarchyid" },
                    { 17L, "image" },
                    { 18L, "money" },
                    { 19L, "nchar(10)" },
                    { 20L, "ntext" },
                    { 21L, "numeric(8,0)" },
                    { 22L, "nvarchar(MAX)" },
                    { 23L, "real" },
                    { 24L, "time(7)" },
                    { 25L, "xml" }
                });

            migrationBuilder.InsertData(
                table: "TypeofReleation",
                columns: new[] { "id", "typerelation" },
                values: new object[,]
                {
                    { 1L, "1-1" },
                    { 2L, "1-n" },
                    { 3L, "m-n" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatabaseLists_Userid",
                table: "DatabaseLists",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelContainer_fieldsofTablesid",
                table: "ModelContainer",
                column: "fieldsofTablesid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelContainer_permitionstoActivitiesid",
                table: "ModelContainer",
                column: "permitionstoActivitiesid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelContainer_relationsofTablesid",
                table: "ModelContainer",
                column: "relationsofTablesid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelContainer_roleid",
                table: "ModelContainer",
                column: "roleid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelContainer_tablesid",
                table: "ModelContainer",
                column: "tablesid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelContainer_typeofFieldid",
                table: "ModelContainer",
                column: "typeofFieldid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelContainer_typeofReleationid",
                table: "ModelContainer",
                column: "typeofReleationid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelContainer_userid",
                table: "ModelContainer",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Roleid",
                table: "Users",
                column: "Roleid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatabaseLists");

            migrationBuilder.DropTable(
                name: "ModelContainer");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "FieldsofTable");

            migrationBuilder.DropTable(
                name: "PermitionstoActivitie");

            migrationBuilder.DropTable(
                name: "RelationsofTable");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "TypeofField");

            migrationBuilder.DropTable(
                name: "TypeofReleation");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
