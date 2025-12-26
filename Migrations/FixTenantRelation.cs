using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreBuilder.Data.Migrations
{
    public partial class FixTenantRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Eðer veritabanýnda EF tarafýndan oluþturulmuþ olabilecek `TenantId1` sütunu varsa kaldýr.
            // Constraint ve index isimleri bilinmediði için güvenli bir SQL bloðu ile önce FK/index varsa düþürülür, sonra sütun silinir.
            migrationBuilder.Sql(@"
IF EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'TenantId1' AND Object_ID = OBJECT_ID(N'dbo.Pages'))
BEGIN
    -- Var ise ilgili foreign key'i bulup kaldýr
    DECLARE @fk NVARCHAR(200);
    SELECT TOP(1) @fk = fk.name
    FROM sys.foreign_keys fk
    JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
    JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
    WHERE fk.parent_object_id = OBJECT_ID(N'dbo.Pages') AND c.name = N'TenantId1';

    IF @fk IS NOT NULL
        EXEC('ALTER TABLE dbo.Pages DROP CONSTRAINT [' + @fk + ']');

    -- Var ise index'i bulup kaldýr
    DECLARE @ix NVARCHAR(200);
    SELECT TOP(1) @ix = i.name
    FROM sys.indexes i
    JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
    JOIN sys.columns c2 ON ic.object_id = c2.object_id AND ic.column_id = c2.column_id
    WHERE i.object_id = OBJECT_ID(N'dbo.Pages') AND c2.name = N'TenantId1';

    IF @ix IS NOT NULL
        EXEC('DROP INDEX [' + @ix + '] ON dbo.Pages');

    ALTER TABLE dbo.Pages DROP COLUMN TenantId1;
END
");

            // Güvence olarak Pages.TenantId üzerinde doðru FK'nýn olduðundan emin olalým.
            // Eðer zaten varsa AddForeignKey hata verebilir; EF migration çalýþtýrdýðýnýz ortamda
            // bu satýr çakýþma çýkarýrsa elle uyarlamanýz gerekebilir.
            try
            {
                migrationBuilder.AddForeignKey(
                    name: "FK_Pages_Tenants_TenantId",
                    table: "Pages",
                    column: "TenantId",
                    principalTable: "Tenants",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }
            catch
            {
                // Eðer zaten varsa migration sýrasýnda hata olmasýn; SQL bloðu yukarý ile temizleme yapýldýysa normalde eklenmelidir.
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Geri alýnýrken TenantId1 sütununu yeniden eklemek riskli olabilir.
            // Basit bir geri alma olarak sütunu yeniden ekle (nullable) — eski veriler kaybolur.
            migrationBuilder.Sql(@"
IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'TenantId1' AND Object_ID = OBJECT_ID(N'dbo.Pages'))
BEGIN
    ALTER TABLE dbo.Pages ADD TenantId1 INT NULL;
END
");
            // Foreign key'i kaldýr (eðer yukarýda eklenmiþse)
            try
            {
                migrationBuilder.DropForeignKey(
                    name: "FK_Pages_Tenants_TenantId",
                    table: "Pages");
            }
            catch
            {
                // ignore
            }
        }
    }
}