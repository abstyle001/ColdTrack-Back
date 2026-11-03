using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColdTrack_Back.Migrations.PostgreSql
{
    /// <inheritdoc />
    public partial class TaskDependencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskDependencies",
                columns: table => new
                {
                    SuccessorTaskId = table.Column<long>(type: "bigint", nullable: false),
                    PredecessorTaskId = table.Column<long>(type: "bigint", nullable: false),
                    DependencyType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDependencies", x => new { x.SuccessorTaskId, x.PredecessorTaskId });
                    table.ForeignKey(
                        name: "FK_TaskDependencies_Tasks_PredecessorTaskId",
                        column: x => x.PredecessorTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskDependencies_Tasks_SuccessorTaskId",
                        column: x => x.SuccessorTaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskDependencies_PredecessorTaskId",
                table: "TaskDependencies",
                column: "PredecessorTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskDependencies");
        }
    }
}
