using System;
using Microsoft.EntityFrameworkCore.Migrations;
using static CITNASDaily.Entities.Enums.Enums;

#nullable disable

namespace CITNASDaily.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivitiesSummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NASId = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    SchoolYear = table.Column<int>(type: "int", nullable: false),
                    DateOfEntry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActivitiesOfTheDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillsLearned = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValuesLearned = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivitiesSummary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyTimeRecord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeOut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OvertimeIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OvertimeOut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalWorkTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Semester = table.Column<int>(type: "int", nullable: true),
                    SchoolYear = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTimeRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Office",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuperiorFirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuperiorLastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Office", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SummaryEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nasId = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    SchoolYear = table.Column<int>(type: "int", nullable: false),
                    SuperiorOverallRating = table.Column<float>(type: "real", nullable: true),
                    AcademicPerformance = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TimekeepingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnrollmentAllowed = table.Column<bool>(type: "bit", nullable: true),
                    UnitsAllowed = table.Column<int>(type: "int", nullable: true),
                    AllCoursesPassed = table.Column<bool>(type: "bit", nullable: true),
                    NoOfCoursesFailed = table.Column<int>(type: "int", nullable: true),
                    Responded = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SummaryEvaluation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimekeepingSummary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NASId = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    SchoolYear = table.Column<int>(type: "int", nullable: false),
                    Excused = table.Column<int>(type: "int", nullable: true),
                    Unexcused = table.Column<int>(type: "int", nullable: true),
                    FailedToPunch = table.Column<int>(type: "int", nullable: true),
                    LateOver10Mins = table.Column<int>(type: "int", nullable: true),
                    LateOver45Mins = table.Column<int>(type: "int", nullable: true),
                    MakeUpDutyHours = table.Column<double>(type: "float", nullable: true),
                    TimekeepingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimekeepingSummary", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Validation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NasId = table.Column<int>(type: "int", nullable: false),
                    NasLetter = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbsenceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    SchoolYear = table.Column<int>(type: "int", nullable: false),
                    ValidationStatus = table.Column<int>(type: "int", nullable: false),
                    MakeUpHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Validation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficeId = table.Column<int>(type: "int", nullable: false),
                    StudentIdNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnNo = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Course = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearLevel = table.Column<int>(type: "int", nullable: false),
                    UnitsAllowed = table.Column<int>(type: "int", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NAS_Office_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Office",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NAS_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OAS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OAS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OAS_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Superior",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OfficeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Superior", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Superior_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NASSchoolYearSemester",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NASId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NASSchoolYear", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NASSchoolYearSemester_NAS_NASId",
                        column: x => x.NASId,
                        principalTable: "NAS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BiometricLog",
                columns: table => new
                {
                    No = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TMNo = table.Column<int>(type: "int", nullable: false),
                    EnNo = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GMNo = table.Column<int>(type: "int", nullable: false),
                    Mode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InOut = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Antipass = table.Column<int>(type: "int", nullable: false),
                    ProxyWork = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NASId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiometricLog", x => x.No);
                    table.ForeignKey(
                        name: "FK_BiometricLog_NAS_NASId",
                        column: x => x.NASId,
                        principalTable: "NAS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NASId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BrokenSched = table.Column<bool>(type: "bit", nullable: false),
                    TotalHours = table.Column<float>(type: "real", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    SchoolYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_NAS_NASId",
                        column: x => x.NASId,
                        principalTable: "NAS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuperiorEvaluationRating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NASId = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    SchoolYear = table.Column<int>(type: "int", nullable: false),
                    AttendanceAndPunctuality = table.Column<int>(type: "int", nullable: false),
                    QualOfWorkOutput = table.Column<int>(type: "int", nullable: false),
                    QuanOfWorkOutput = table.Column<int>(type: "int", nullable: false),
                    AttitudeAndWorkBehaviour = table.Column<int>(type: "int", nullable: false),
                    OverallAssessment = table.Column<int>(type: "int", nullable: false),
                    OverallRating = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperiorEvaluationRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuperiorEvaluationRating_NAS_NASId",
                        column: x => x.NASId,
                        principalTable: "NAS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BiometricLog_NASId",
                table: "BiometricLog",
                column: "NASId");

            migrationBuilder.CreateIndex(
                name: "IX_NAS_OfficeId",
                table: "NAS",
                column: "OfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_NAS_UserId",
                table: "NAS",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OAS_UserId",
                table: "OAS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_NASId",
                table: "Schedule",
                column: "NASId");

            migrationBuilder.CreateIndex(
                name: "IX_Superior_UserId",
                table: "Superior",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SuperiorEvaluationRating_NASId",
                table: "SuperiorEvaluationRating",
                column: "NASId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NASSchoolYearSemester");

            migrationBuilder.DropTable(
                name: "ActivitiesSummary");

            migrationBuilder.DropTable(
                name: "BiometricLog");

            migrationBuilder.DropTable(
                name: "DailyTimeRecord");

            migrationBuilder.DropTable(
                name: "OAS");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "SummaryEvaluation");

            migrationBuilder.DropTable(
                name: "Superior");

            migrationBuilder.DropTable(
                name: "SuperiorEvaluationRating");

            migrationBuilder.DropTable(
                name: "TimekeepingSummary");

            migrationBuilder.DropTable(
                name: "Validation");

            migrationBuilder.DropTable(
                name: "NAS");

            migrationBuilder.DropTable(
                name: "Office");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
