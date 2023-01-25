using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class ChangePartners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "streetcode_partner",
                schema: "streetcode");

            migrationBuilder.AddColumn<bool>(
                name: "IsKeyPartner",
                schema: "partners",
                table: "partners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "streetcode_partners",
                schema: "streetcode",
                columns: table => new
                {
                    PartnersId = table.Column<int>(type: "int", nullable: false),
                    StreetcodesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_partners", x => new { x.PartnersId, x.StreetcodesId });
                    table.ForeignKey(
                        name: "FK_streetcode_partners_partners_PartnersId",
                        column: x => x.PartnersId,
                        principalSchema: "partners",
                        principalTable: "partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_partners_streetcodes_StreetcodesId",
                        column: x => x.StreetcodesId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 1,
                column: "Url",
                value: "http://www.univ.kiev.ua/tpl/img/photo-osobystosti/foto-shevchenko.jpg");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 5,
                column: "Url",
                value: "https://www.megakniga.com.ua/uploads/cache/Products/Product_images_343456/d067b1_w1600.jpg");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 6,
                column: "Url",
                value: "https://upload.wikimedia.org/wikipedia/commons/2/21/PGRS_2_051_Kostomarov_-_crop.jpg");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 7,
                column: "Url",
                value: "https://upload.wikimedia.org/wikipedia/commons/6/6a/%D0%91%D0%B5%D0%BB%D0%BE%D0%B7%D0%B5%D1%80%D1%81%D0%BA%D0%B8%D0%B9_%D0%92%D0%B0%D1%81%D0%B8%D0%BB%D0%B8%D0%B9.JPG");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 8,
                column: "Url",
                value: "https://img.tsn.ua/cached/907/tsn-15890496c3fba55a55e21f0ca3090d06/thumbs/x/3e/1a/97fe20f34f78c6f13ea84dbf15ee1a3e.jpeg");

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partners",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsKeyPartner",
                value: true);

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 25, 12, 28, 0, 400, DateTimeKind.Local).AddTicks(8169));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 25, 12, 28, 0, 400, DateTimeKind.Local).AddTicks(8035));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 25, 12, 28, 0, 400, DateTimeKind.Local).AddTicks(8112));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 25, 12, 28, 0, 400, DateTimeKind.Local).AddTicks(8119));

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_partners_StreetcodesId",
                schema: "streetcode",
                table: "streetcode_partners",
                column: "StreetcodesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "streetcode_partners",
                schema: "streetcode");

            migrationBuilder.DropColumn(
                name: "IsKeyPartner",
                schema: "partners",
                table: "partners");

            migrationBuilder.CreateTable(
                name: "streetcode_partner",
                schema: "streetcode",
                columns: table => new
                {
                    PartnerId = table.Column<int>(type: "int", nullable: false),
                    StreetcodeId = table.Column<int>(type: "int", nullable: false),
                    IsSponsor = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_streetcode_partner", x => new { x.PartnerId, x.StreetcodeId });
                    table.ForeignKey(
                        name: "FK_streetcode_partner_partners_PartnerId",
                        column: x => x.PartnerId,
                        principalSchema: "partners",
                        principalTable: "partners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_streetcode_partner_streetcodes_StreetcodeId",
                        column: x => x.StreetcodeId,
                        principalSchema: "streetcode",
                        principalTable: "streetcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 1,
                column: "Url",
                value: "https://www.bing.com/images/search?view=detailV2&ccid=07Ymnt6l&id=A8ACFBEB6A3695B1E7DD2887A46505D759921BDC&thid=OIP.07Ymnt6ljB86Jx-Fy2wGUwHaKY&mediaurl=https%3A%2F%2Fproprikol.ru%2Fwp-content%2Fuploads%2F2021%2F05%2Fkartinki-taras-shevchenko-39.jpg&cdnurl=https%3A%2F%2Fth.bing.com%2Fth%2Fid%2FR.d3b6269edea58c1f3a271f85cb6c0653%3Frik%3D3BuSWdcFZaSHKA%26pid%3DImgRaw%26r%3D0&exph=1581&expw=1128&q=%d1%82%d0%b0%d1%80%d0%b0%d1%81+%d1%88%d0%b5%d0%b2%d1%87%d0%b5%d0%bd%d0%ba%d0%be&simid=608002717784548528&form=IRPRST&ck=0EF4CC2DA9612AC10DAE96953F62051F&selectedindex=1&ajaxhist=0&ajaxserp=0&vt=0&sim=11");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 5,
                column: "Url",
                value: "https://www.bing.com/images/search?view=detailV2&ccid=6juPycgD&id=00A2C7B1F325A9870421D651A956BCE2C851654E&thid=OIP.6juPycgDNwJ3v2Zr-kde1gHaK_&mediaurl=https%3A%2F%2Fwww.megakniga.com.ua%2Fuploads%2Fcache%2FProducts%2FProduct_images_343456%2Fd067b1_w1600.jpg&cdnurl=https%3A%2F%2Fth.bing.com%2Fth%2Fid%2FR.ea3b8fc9c803370277bf666bfa475ed6%3Frik%3DTmVRyOK8VqlR1g%26pid%3DImgRaw%26r%3D0&exph=1200&expw=809&q=%d0%ba%d0%be%d0%b1%d0%b7%d0%b0%d1%80&simid=608047540067197142&form=IRPRST&ck=4280C365AEBC65D796FBF885B3252710&selectedindex=1&ajaxhist=0&ajaxserp=0&vt=0&sim=11");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 6,
                column: "Url",
                value: "https://www.bing.com/images/search?view=detailV2&ccid=KUJZwRaU&id=A53DDEBFF57BE2396FB7FA50737F83704B1BE30F&thid=OIP.KUJZwRaUjipKMLR8H91BrAAAAA&mediaurl=https%3a%2f%2fgdb.rferl.org%2f224F2B76-EE74-4B85-A78A-BF8A354FA0B1_w250_r0_s.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.294259c116948e2a4a30b47c1fdd41ac%3frik%3dD%252bMbS3CDf3NQ%252bg%26pid%3dImgRaw%26r%3d0&exph=340&expw=250&q=%d0%9c%d0%b8%d0%ba%d0%be%cc%81%d0%bb%d0%b0+%d0%9a%d0%be%d1%81%d1%82%d0%be%d0%bc%d0%b0%cc%81%d1%80%d0%be%d0%b2&simid=608030609289524022&FORM=IRPRST&ck=E08972A7A7E2CEE9B67158DDC372F92F&selectedIndex=3&ajaxhist=0&ajaxserp=0");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 7,
                column: "Url",
                value: "https://www.bing.com/images/search?view=detailV2&ccid=hIQUFjAM&id=B14676F51B4A0EB314ED15283540D088B3030E28&thid=OIP.hIQUFjAMGwOt7f7ujR44aQAAAA&mediaurl=https%3a%2f%2fnaurok-test.nyc3.cdn.digitaloceanspaces.com%2fuploads%2ftest%2f229691%2f36505%2f276576_1582512990.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.84841416300c1b03adedfeee8d1e3869%3frik%3dKA4Ds4jQQDUoFQ%26pid%3dImgRaw%26r%3d0&exph=351&expw=240&q=%d0%92%d0%b0%d1%81%d0%b8%d0%bb%d1%8c+%d0%91%d1%96%d0%bb%d0%be%d0%b7%d0%b5%d1%80%d1%81%d1%8c%d0%ba%d0%b8%d0%b9&simid=608001205960330039&FORM=IRPRST&ck=07DE282212732F4C0712D614C87002F3&selectedIndex=1&ajaxhist=0&ajaxserp=0");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 8,
                column: "Url",
                value: "https://www.bing.com/images/search?view=detailV2&ccid=F5o3vrW9&id=5409686EF1396243251CE5AF505766A0A2D0662E&thid=OIP.F5o3vrW9jZJ9ECMgkmevTwHaFj&mediaurl=https%3a%2f%2fstorage1.censor.net%2fimages%2f1%2f7%2f9%2fa%2f179a37beb5bd8d927d1023209267af4f%2foriginal.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.179a37beb5bd8d927d1023209267af4f%3frik%3dLmbQoqBmV1Cv5Q%26pid%3dImgRaw%26r%3d0&exph=720&expw=960&q=%d0%b2%d0%b8%d0%b7%d0%b2%d0%be%d0%bb%d0%b5%d0%bd%d0%bd%d1%8f+%d1%85%d0%b5%d1%80%d1%81%d0%be%d0%bd%d1%83&simid=608050323200235844&FORM=IRPRST&ck=C9A86B9D5EBBADF456F315DFD0BA990B&selectedIndex=3&ajaxhist=0&ajaxserp=0");

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcode_partner",
                columns: new[] { "PartnerId", "StreetcodeId", "IsSponsor" },
                values: new object[,]
                {
                    { 1, 1, true },
                    { 1, 2, true },
                    { 1, 3, false },
                    { 1, 4, true },
                    { 2, 1, true },
                    { 2, 2, false },
                    { 2, 4, true },
                    { 3, 3, false }
                });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 17, 21, 40, 17, 968, DateTimeKind.Local).AddTicks(4481));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 17, 21, 40, 17, 968, DateTimeKind.Local).AddTicks(4412));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 17, 21, 40, 17, 968, DateTimeKind.Local).AddTicks(4459));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 17, 21, 40, 17, 968, DateTimeKind.Local).AddTicks(4463));

            migrationBuilder.CreateIndex(
                name: "IX_streetcode_partner_StreetcodeId",
                schema: "streetcode",
                table: "streetcode_partner",
                column: "StreetcodeId");
        }
    }
}
