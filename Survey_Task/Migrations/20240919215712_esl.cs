using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Survey_Task.Migrations
{
    /// <inheritdoc />
    public partial class esl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Surveys_SurveyId",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "SelectedDays",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "SelectedMonths",
                table: "Surveys");

            migrationBuilder.DropColumn(
                name: "SelectedWeekDays",
                table: "Surveys");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Ratings",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "Item",
                table: "Ratings",
                newName: "ItemName");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_SurveyId",
                table: "Ratings",
                newName: "IX_Ratings_SurveyId");

            migrationBuilder.AlterColumn<int>(
                name: "SurveyId",
                table: "Ratings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Surveys_SurveyId",
                table: "Ratings",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Surveys_SurveyId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Rating",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "ItemName",
                table: "Rating",
                newName: "Item");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_SurveyId",
                table: "Rating",
                newName: "IX_Rating_SurveyId");

            migrationBuilder.AddColumn<string>(
                name: "SelectedDays",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SelectedMonths",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SelectedWeekDays",
                table: "Surveys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "SurveyId",
                table: "Rating",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Surveys_SurveyId",
                table: "Rating",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
