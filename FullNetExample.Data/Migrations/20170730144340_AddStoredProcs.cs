using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FullNetExample.Data.Migrations
{
    public partial class AddStoredProcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
            name: "IX_SamuraiBattles_BattleId",
            table: "SamuraiBattles");

            migrationBuilder.Sql(
                @"CREATE PROCEDURE FilterSamuraiByNamePart
                  @namepart varchar(50) 
                  AS
                  select * from samurais where name like '%'+@namepart+'%'");

            migrationBuilder.Sql(
              @"create procedure FindLongestName
              @procResult varchar(50) OUTPUT
              AS
              BEGIN
              SET NOCOUNT ON;
              select top 1 @procResult= name from samurais order by len(name) desc
              END"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
               name: "IX_SamuraiBattles_BattleId",
               table: "SamuraiBattles",
               column: "BattleId");
            migrationBuilder.Sql("DROP PROCEDURE FindLongestName");
            migrationBuilder.Sql("drop procedure FilterSamuraiByNamePart");
        }
    }
}
