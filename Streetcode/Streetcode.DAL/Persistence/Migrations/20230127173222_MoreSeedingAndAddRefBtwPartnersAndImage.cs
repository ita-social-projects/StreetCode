using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class MoreSeedingAndAddRefBtwPartnersAndImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                schema: "partners",
                table: "partners");

            migrationBuilder.AddColumn<int>(
                name: "LogoId",
                schema: "partners",
                table: "partners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                schema: "media",
                table: "images",
                columns: new[] { "Id", "Alt", "Title", "Url" },
                values: new object[,]
                {
                    { 12, "SoftServe", "SoftServe", "https://itukraine.org.ua/files/img/illus/members/softserve%20logo.png" },
                    { 13, "Parimatch", "Parimatch", "https://static.ua-football.com/img/upload/19/270071.png" },
                    { 14, "Community Partners", "Community Partners", "https://communitypartnersinc.org/wp-content/uploads/2018/03/CP_Logo_RGB_Horizontal-e1520810390513.png" },
                    { 15, "Володимир-Варфоломей", "Володимир-Варфоломей", "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2d/Ecumenical_Patriarch_Bartholomew_in_the_Vatican_2021_%28cropped%29.jpg/800px-Ecumenical_Patriarch_Bartholomew_in_the_Vatican_2021_%28cropped%29.jpg" },
                    { 16, "Леся Українка", "Леся Українка", "https://api.culture.pl/sites/default/files/styles/embed_image_360/public/2022-03/lesya_ukrainka_portrait_public_domain.jpg?itok=1jAIv48D" },
                    { 17, "Іван Мазепа", "Іван Мазепа", "https://reibert.info/attachments/hetmans_catalog-1-4-scaled-jpg.18981447/" }
                });

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 1,
                column: "LogoUrl",
                value: "https://play-lh.googleusercontent.com/kMofEFLjobZy_bCuaiDogzBcUT-dz3BBbOrIEjJ-hqOabjK8ieuevGe6wlTD15QzOqw");

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 2,
                column: "LogoUrl",
                value: "https://www.facebook.com/images/fb_icon_325x325.png");

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 3,
                column: "LogoUrl",
                value: "https://upload.wikimedia.org/wikipedia/commons/thumb/9/95/Instagram_logo_2022.svg/1200px-Instagram_logo_2022.svg.png");

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "related_figures",
                columns: new[] { "ObserverId", "TargetId" },
                values: new object[] { 1, 4 });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 27, 19, 32, 21, 132, DateTimeKind.Local).AddTicks(190));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 27, 19, 32, 21, 132, DateTimeKind.Local).AddTicks(2));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 27, 19, 32, 21, 132, DateTimeKind.Local).AddTicks(86));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 1, 27, 19, 32, 21, 132, DateTimeKind.Local).AddTicks(93));

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcodes",
                columns: new[] { "Id", "CreatedAt", "EventEndOrPersonDeathDate", "EventStartOrPersonBirthDate", "FirstName", "Index", "LastName", "Rank", "StreetcodeType", "Teaser" },
                values: new object[,]
                {
                    { 5, new DateTime(2023, 1, 27, 19, 32, 21, 132, DateTimeKind.Local).AddTicks(100), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Володимир-Варфоломей", 5, "Кропивницький-Шевченківський", null, "streetcode-person", "some teaser" },
                    { 6, new DateTime(2023, 1, 27, 19, 32, 21, 132, DateTimeKind.Local).AddTicks(131), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Леся", 6, "Українка", null, "streetcode-person", "some teaser" },
                    { 7, new DateTime(2023, 1, 27, 19, 32, 21, 132, DateTimeKind.Local).AddTicks(138), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Іван", 7, "Мазепа", null, "streetcode-person", "some teaser" }
                });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "tags",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 5, "Наукова школа" },
                    { 6, "Історія" },
                    { 7, "Політика" }
                });

            migrationBuilder.InsertData(
                schema: "timeline",
                table: "timeline_items",
                columns: new[] { "Id", "Date", "Description", "Title" },
                values: new object[,]
                {
                    { 3, new DateTime(1832, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 4, new DateTime(1833, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 5, new DateTime(1834, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 6, new DateTime(1834, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 7, new DateTime(1834, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 8, new DateTime(1834, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 9, new DateTime(1835, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" },
                    { 10, new DateTime(1836, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.", "Перші роки в Петербурзі" }
                });

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partners",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "LogoId" },
                values: new object[] { "Український культурний фонд є флагманською українською інституцією культури, яка у своїй діяльності інтегрує різні види мистецтва – від сучасного мистецтва, нової музики й театру до літератури та музейної справи. Мистецький арсенал є флагманською українською інституцією культури, яка у своїй діяльності інтегрує різні види мистецтва – від сучасного мистецтва, нової музики й театру до літератури та музейної справи.", 12 });

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partners",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "LogoId", "Title" },
                values: new object[] { "some text", 13, "Parimatch" });

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partners",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LogoId", "TargetUrl" },
                values: new object[] { 14, "https://partners.salesforce.com/pdx/s/?language=en_US&redirected=RGSUDODQUL" });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "related_figures",
                columns: new[] { "ObserverId", "TargetId" },
                values: new object[,]
                {
                    { 5, 1 },
                    { 6, 1 },
                    { 7, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_partners_LogoId",
                schema: "partners",
                table: "partners",
                column: "LogoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_partners_images_LogoId",
                schema: "partners",
                table: "partners",
                column: "LogoId",
                principalSchema: "media",
                principalTable: "images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_partners_images_LogoId",
                schema: "partners",
                table: "partners");

            migrationBuilder.DropIndex(
                name: "IX_partners_LogoId",
                schema: "partners",
                table: "partners");

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "related_figures",
                keyColumns: new[] { "ObserverId", "TargetId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "related_figures",
                keyColumns: new[] { "ObserverId", "TargetId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "related_figures",
                keyColumns: new[] { "ObserverId", "TargetId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "related_figures",
                keyColumns: new[] { "ObserverId", "TargetId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "tags",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "tags",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "tags",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "timeline",
                table: "timeline_items",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "timeline",
                table: "timeline_items",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "timeline",
                table: "timeline_items",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "timeline",
                table: "timeline_items",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "timeline",
                table: "timeline_items",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "timeline",
                table: "timeline_items",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "timeline",
                table: "timeline_items",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "timeline",
                table: "timeline_items",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "LogoId",
                schema: "partners",
                table: "partners");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                schema: "partners",
                table: "partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 1,
                column: "LogoUrl",
                value: "");

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 2,
                column: "LogoUrl",
                value: "");

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partner_source_links",
                keyColumn: "Id",
                keyValue: 3,
                column: "LogoUrl",
                value: "");

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partners",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "LogoUrl" },
                values: new object[] { "Developers", "https://www.bing.com/images/search?view=detailV2&ccid=g3DnkGqg&id=98C6F1FDD6CDA685A3DE2AD392FAC228180A28CC&thid=OIP.g3DnkGqgmhKFWM2ct5mXrAHaHa&mediaurl=https%3a%2f%2fyt3.ggpht.com%2fa-%2fAN66SAxaiWXvFxW9BUQ32pzQ5tv5UuXz2fLZ20LaMg%3ds900-mo-c-c0xffffffff-rj-k-no&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.8370e7906aa09a128558cd9cb79997ac%3frik%3dzCgKGCjC%252bpLTKg%26pid%3dImgRaw%26r%3d0&exph=900&expw=900&q=softserve&simid=608013145967840441&FORM=IRPRST&ck=C08BED6E397D35D8A4824BB4B78EBCE8&selectedIndex=1&ajaxhist=0&ajaxserp=0" });

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partners",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "LogoUrl", "Title" },
                values: new object[] { null, "https://www.bing.com/images/search?view=detailV2&ccid=9TObzn%2ba&id=21D8755FE7846CE9660BC2365F5EE70417D31DA7&thid=OIP.9TObzn-a15MsLhdfHh1e_gHaE8&mediaurl=https%3a%2f%2fi2.wp.com%2feuropeangaming.eu%2fportal%2fwp-content%2fuploads%2f2020%2f02%2f5-10.jpg%3ffit%3d1200%252C800%26ssl%3d1&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.f5339bce7f9ad7932c2e175f1e1d5efe%3frik%3dpx3TFwTnXl82wg%26pid%3dImgRaw%26r%3d0&exph=800&expw=1200&q=parimatch&simid=607987165708116105&FORM=IRPRST&ck=BA1164F39CC2BBD1CE20F50A93602E5C&selectedIndex=1&ajaxhist=0&ajaxserp=0", "parimatch" });

            migrationBuilder.UpdateData(
                schema: "partners",
                table: "partners",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "LogoUrl", "TargetUrl" },
                values: new object[] { "https://www.bing.com/images/search?view=detailV2&ccid=9TObzn%2ba&id=21D8755FE7846CE9660BC2365F5EE70417D31DA7&thid=OIP.9TObzn-a15MsLhdfHh1e_gHaE8&mediaurl=https%3a%2f%2fi2.wp.com%2feuropeangaming.eu%2fportal%2fwp-content%2fuploads%2f2020%2f02%2f5-10.jpg%3ffit%3d1200%252C800%26ssl%3d1&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.f5339bce7f9ad7932c2e175f1e1d5efe%3frik%3dpx3TFwTnXl82wg%26pid%3dImgRaw%26r%3d0&exph=800&expw=1200&q=parimatch&simid=607987165708116105&FORM=IRPRST&ck=BA1164F39CC2BBD1CE20F50A93602E5C&selectedIndex=1&ajaxhist=0&ajaxserp=0", "https://parimatch.com/" });

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
        }
    }
}
