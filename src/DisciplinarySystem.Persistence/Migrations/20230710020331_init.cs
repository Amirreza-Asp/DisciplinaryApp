using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisciplinarySystem.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthRole", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Complainings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationalGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    College = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Father = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complainings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plaintiffs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plaintiffs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Verdicts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verdicts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViolationCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolationCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proceedings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    MeetingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proceedings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Proceedings_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComplainingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaintiffId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complaints_Complainings_ComplainingId",
                        column: x => x.ComplainingId,
                        principalTable: "Complainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Complaints_Plaintiffs_PlaintiffId",
                        column: x => x.PlaintiffId,
                        principalTable: "Plaintiffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cases",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ComplaintId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cases_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComplaintId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComplaintDocuments_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: false),
                    NationalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Family = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthUser_AuthRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AuthRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MeetingUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeetingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingUsers", x => new { x.UserId, x.MeetingId });
                    table.ForeignKey(
                        name: "FK_MeetingUsers_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetingUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Defences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defences_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Epistles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reciver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseId = table.Column<long>(type: "bigint", nullable: true),
                    ComplaintId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Epistles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Epistles_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Epistles_Complaints_ComplaintId",
                        column: x => x.ComplaintId,
                        principalTable: "Complaints",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Informants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Father = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Statements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Informants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Informants_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseId = table.Column<long>(type: "bigint", nullable: false),
                    ComplainingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlaintiffId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetDate()"),
                    InviteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invitations_Complainings_ComplainingId",
                        column: x => x.ComplainingId,
                        principalTable: "Complainings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Invitations_Plaintiffs_PlaintiffId",
                        column: x => x.PlaintiffId,
                        principalTable: "Plaintiffs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Objections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Objections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Objections_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedInfos_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Violations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Vote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Violations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Violations_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Violations_ViolationCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ViolationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SMS",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(13)", unicode: false, maxLength: 13, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SMS_AuthUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DefenceDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefenceDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DefenceDocuments_Defences_DefenceId",
                        column: x => x.DefenceId,
                        principalTable: "Defences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EpistleDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EpistleId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpistleDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpistleDocuments_Epistles_EpistleId",
                        column: x => x.EpistleId,
                        principalTable: "Epistles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InformantsDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InformedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformantsDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InformantsDocuments_Informants_InformedId",
                        column: x => x.InformedId,
                        principalTable: "Informants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvitationDocument",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitationDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvitationDocument_Invitations_InvitationId",
                        column: x => x.InvitationId,
                        principalTable: "Invitations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvitationUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitationUsers", x => new { x.UserId, x.InvitationId });
                    table.ForeignKey(
                        name: "FK_InvitationUsers_Invitations_InvitationId",
                        column: x => x.InvitationId,
                        principalTable: "Invitations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvitationUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ObjectionDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObjectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectionDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ObjectionDocuments_Objections_ObjectionId",
                        column: x => x.ObjectionId,
                        principalTable: "Objections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatedInfoDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedInfoDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedInfoDocuments_RelatedInfos_RelatedInfoId",
                        column: x => x.RelatedInfoId,
                        principalTable: "RelatedInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CentralCommitteeVote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    VerdictId = table.Column<long>(type: "bigint", nullable: false),
                    ViolationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentralCommitteeVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentralCommitteeVote_Verdicts_VerdictId",
                        column: x => x.VerdictId,
                        principalTable: "Verdicts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CentralCommitteeVote_Violations_ViolationId",
                        column: x => x.ViolationId,
                        principalTable: "Violations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinalVotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    VerdictId = table.Column<long>(type: "bigint", nullable: false),
                    ViolationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalVotes_Verdicts_VerdictId",
                        column: x => x.VerdictId,
                        principalTable: "Verdicts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinalVotes_Violations_ViolationId",
                        column: x => x.ViolationId,
                        principalTable: "Violations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryVotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    VerdictId = table.Column<long>(type: "bigint", nullable: false),
                    ViolationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryVotes_Verdicts_VerdictId",
                        column: x => x.VerdictId,
                        principalTable: "Verdicts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrimaryVotes_Violations_ViolationId",
                        column: x => x.ViolationId,
                        principalTable: "Violations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViolationDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViolationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViolationDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ViolationDocuments_Violations_ViolationId",
                        column: x => x.ViolationId,
                        principalTable: "Violations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CentralCommitteeVoteDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CentralCommitteeVoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentralCommitteeVoteDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CentralCommitteeVoteDocuments_CentralCommitteeVote_CentralCommitteeVoteId",
                        column: x => x.CentralCommitteeVoteId,
                        principalTable: "CentralCommitteeVote",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinalVoteDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinalVoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalVoteDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinalVoteDocuments_FinalVotes_FinalVoteId",
                        column: x => x.FinalVoteId,
                        principalTable: "FinalVotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryVoteDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrimaryVoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    File = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryVoteDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrimaryVoteDocuments_PrimaryVotes_PrimaryVoteId",
                        column: x => x.PrimaryVoteId,
                        principalTable: "PrimaryVotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthUser_RoleId",
                table: "AuthUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthUser_UserId",
                table: "AuthUser",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ComplaintId",
                table: "Cases",
                column: "ComplaintId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CentralCommitteeVote_VerdictId",
                table: "CentralCommitteeVote",
                column: "VerdictId");

            migrationBuilder.CreateIndex(
                name: "IX_CentralCommitteeVote_ViolationId",
                table: "CentralCommitteeVote",
                column: "ViolationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CentralCommitteeVoteDocuments_CentralCommitteeVoteId",
                table: "CentralCommitteeVoteDocuments",
                column: "CentralCommitteeVoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintDocuments_ComplaintId",
                table: "ComplaintDocuments",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_ComplainingId",
                table: "Complaints",
                column: "ComplainingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_PlaintiffId",
                table: "Complaints",
                column: "PlaintiffId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DefenceDocuments_DefenceId",
                table: "DefenceDocuments",
                column: "DefenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Defences_CaseId",
                table: "Defences",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_EpistleDocuments_EpistleId",
                table: "EpistleDocuments",
                column: "EpistleId");

            migrationBuilder.CreateIndex(
                name: "IX_Epistles_CaseId",
                table: "Epistles",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Epistles_ComplaintId",
                table: "Epistles",
                column: "ComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalVoteDocuments_FinalVoteId",
                table: "FinalVoteDocuments",
                column: "FinalVoteId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalVotes_VerdictId",
                table: "FinalVotes",
                column: "VerdictId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalVotes_ViolationId",
                table: "FinalVotes",
                column: "ViolationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Informants_CaseId",
                table: "Informants",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_InformantsDocuments_InformedId",
                table: "InformantsDocuments",
                column: "InformedId");

            migrationBuilder.CreateIndex(
                name: "IX_InvitationDocument_InvitationId",
                table: "InvitationDocument",
                column: "InvitationId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_CaseId",
                table: "Invitations",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ComplainingId",
                table: "Invitations",
                column: "ComplainingId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_PlaintiffId",
                table: "Invitations",
                column: "PlaintiffId");

            migrationBuilder.CreateIndex(
                name: "IX_InvitationUsers_InvitationId",
                table: "InvitationUsers",
                column: "InvitationId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetingUsers_MeetingId",
                table: "MeetingUsers",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectionDocuments_ObjectionId",
                table: "ObjectionDocuments",
                column: "ObjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Objections_CaseId",
                table: "Objections",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryVoteDocuments_PrimaryVoteId",
                table: "PrimaryVoteDocuments",
                column: "PrimaryVoteId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryVotes_VerdictId",
                table: "PrimaryVotes",
                column: "VerdictId");

            migrationBuilder.CreateIndex(
                name: "IX_PrimaryVotes_ViolationId",
                table: "PrimaryVotes",
                column: "ViolationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proceedings_MeetingId",
                table: "Proceedings",
                column: "MeetingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RelatedInfoDocuments_RelatedInfoId",
                table: "RelatedInfoDocuments",
                column: "RelatedInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedInfos_CaseId",
                table: "RelatedInfos",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_SMS_UserId",
                table: "SMS",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ViolationDocuments_ViolationId",
                table: "ViolationDocuments",
                column: "ViolationId");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_CaseId",
                table: "Violations",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Violations_CategoryId",
                table: "Violations",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CentralCommitteeVoteDocuments");

            migrationBuilder.DropTable(
                name: "ComplaintDocuments");

            migrationBuilder.DropTable(
                name: "DefenceDocuments");

            migrationBuilder.DropTable(
                name: "EpistleDocuments");

            migrationBuilder.DropTable(
                name: "FinalVoteDocuments");

            migrationBuilder.DropTable(
                name: "InformantsDocuments");

            migrationBuilder.DropTable(
                name: "InvitationDocument");

            migrationBuilder.DropTable(
                name: "InvitationUsers");

            migrationBuilder.DropTable(
                name: "MeetingUsers");

            migrationBuilder.DropTable(
                name: "ObjectionDocuments");

            migrationBuilder.DropTable(
                name: "PrimaryVoteDocuments");

            migrationBuilder.DropTable(
                name: "Proceedings");

            migrationBuilder.DropTable(
                name: "RelatedInfoDocuments");

            migrationBuilder.DropTable(
                name: "SMS");

            migrationBuilder.DropTable(
                name: "ViolationDocuments");

            migrationBuilder.DropTable(
                name: "CentralCommitteeVote");

            migrationBuilder.DropTable(
                name: "Defences");

            migrationBuilder.DropTable(
                name: "Epistles");

            migrationBuilder.DropTable(
                name: "FinalVotes");

            migrationBuilder.DropTable(
                name: "Informants");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "Objections");

            migrationBuilder.DropTable(
                name: "PrimaryVotes");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "RelatedInfos");

            migrationBuilder.DropTable(
                name: "AuthUser");

            migrationBuilder.DropTable(
                name: "Verdicts");

            migrationBuilder.DropTable(
                name: "Violations");

            migrationBuilder.DropTable(
                name: "AuthRole");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Cases");

            migrationBuilder.DropTable(
                name: "ViolationCategories");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "Complainings");

            migrationBuilder.DropTable(
                name: "Plaintiffs");
        }
    }
}
