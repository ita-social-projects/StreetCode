using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFTask.Migrations
{
    public partial class ChangeSeedingId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                schema: "media",
                table: "audios",
                columns: new[] { "Id", "Description", "StreetcodeId", "Title", "Url" },
                values: new object[] { 4, "for streetcode4", 4, "audio4", "https://somelink4" });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "coordinates",
                columns: new[] { "Id", "Latitude", "Longtitude", "StreetcodeId", "coordinate_type" },
                values: new object[] { 10, 46.3950m, 32.3738m, 4, "coordinate_streetcode" });

            migrationBuilder.InsertData(
                schema: "media",
                table: "images",
                columns: new[] { "Id", "Alt", "Title", "Url" },
                values: new object[,]
                {
                    { 6, "Мико́ла Костома́ров", "Мико́ла Костома́ров", "https://www.bing.com/images/search?view=detailV2&ccid=KUJZwRaU&id=A53DDEBFF57BE2396FB7FA50737F83704B1BE30F&thid=OIP.KUJZwRaUjipKMLR8H91BrAAAAA&mediaurl=https%3a%2f%2fgdb.rferl.org%2f224F2B76-EE74-4B85-A78A-BF8A354FA0B1_w250_r0_s.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.294259c116948e2a4a30b47c1fdd41ac%3frik%3dD%252bMbS3CDf3NQ%252bg%26pid%3dImgRaw%26r%3d0&exph=340&expw=250&q=%d0%9c%d0%b8%d0%ba%d0%be%cc%81%d0%bb%d0%b0+%d0%9a%d0%be%d1%81%d1%82%d0%be%d0%bc%d0%b0%cc%81%d1%80%d0%be%d0%b2&simid=608030609289524022&FORM=IRPRST&ck=E08972A7A7E2CEE9B67158DDC372F92F&selectedIndex=3&ajaxhist=0&ajaxserp=0" },
                    { 7, "Василь Білозерський", "Василь Білозерський", "https://www.bing.com/images/search?view=detailV2&ccid=hIQUFjAM&id=B14676F51B4A0EB314ED15283540D088B3030E28&thid=OIP.hIQUFjAMGwOt7f7ujR44aQAAAA&mediaurl=https%3a%2f%2fnaurok-test.nyc3.cdn.digitaloceanspaces.com%2fuploads%2ftest%2f229691%2f36505%2f276576_1582512990.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.84841416300c1b03adedfeee8d1e3869%3frik%3dKA4Ds4jQQDUoFQ%26pid%3dImgRaw%26r%3d0&exph=351&expw=240&q=%d0%92%d0%b0%d1%81%d0%b8%d0%bb%d1%8c+%d0%91%d1%96%d0%bb%d0%be%d0%b7%d0%b5%d1%80%d1%81%d1%8c%d0%ba%d0%b8%d0%b9&simid=608001205960330039&FORM=IRPRST&ck=07DE282212732F4C0712D614C87002F3&selectedIndex=1&ajaxhist=0&ajaxserp=0" },
                    { 8, "Звільнення Херсона", "Звільнення Херсона", "https://www.bing.com/images/search?view=detailV2&ccid=F5o3vrW9&id=5409686EF1396243251CE5AF505766A0A2D0662E&thid=OIP.F5o3vrW9jZJ9ECMgkmevTwHaFj&mediaurl=https%3a%2f%2fstorage1.censor.net%2fimages%2f1%2f7%2f9%2fa%2f179a37beb5bd8d927d1023209267af4f%2foriginal.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.179a37beb5bd8d927d1023209267af4f%3frik%3dLmbQoqBmV1Cv5Q%26pid%3dImgRaw%26r%3d0&exph=720&expw=960&q=%d0%b2%d0%b8%d0%b7%d0%b2%d0%be%d0%bb%d0%b5%d0%bd%d0%bd%d1%8f+%d1%85%d0%b5%d1%80%d1%81%d0%be%d0%bd%d1%83&simid=608050323200235844&FORM=IRPRST&ck=C9A86B9D5EBBADF456F315DFD0BA990B&selectedIndex=3&ajaxhist=0&ajaxserp=0" }
                });

            migrationBuilder.InsertData(
                schema: "sources",
                table: "source_links",
                columns: new[] { "Id", "StreetcodeId", "Title", "Url" },
                values: new object[] { 3, 4, "Св'яткування звільнення", "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html" });

            migrationBuilder.InsertData(
                schema: "partner_sponsors",
                table: "streetcode_partners",
                columns: new[] { "PartnerId", "StreetcodeId", "IsSponsor" },
                values: new object[,]
                {
                    { 1, 4, true },
                    { 2, 4, true }
                });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreateDate", "EventEndOrPersonDeathDate", "EventStartOrPersonBirthDate", "Index", "Teaser", "Title", "ViewCount" },
                values: new object[] { new DateTime(2022, 12, 12, 20, 18, 50, 642, DateTimeKind.Local).AddTicks(7816), new DateTime(2022, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 11, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Звільнення Херсона (11 листопада 2022) — відвоювання Збройними силами України (ЗСУ) міста Херсона та інших районів Херсонської області та частини Миколаївської області на правому березі Дніпра, тоді як збройні сили РФ Сили відійшли на лівий берег (відомий як відхід росіян з Херсона, 9–11 листопада 2022 р.).", "Звільнення Херсона", 1000 });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcodes",
                columns: new[] { "Id", "CreateDate", "EventEndOrPersonDeathDate", "EventStartOrPersonBirthDate", "FirstName", "Index", "LastName", "MiddleName", "Teaser", "streetcode_type" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 20, 18, 50, 642, DateTimeKind.Local).AddTicks(7668), new DateTime(1861, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1814, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Тарас", 1, "Шевченко", "Григорович", "Тара́с Григо́рович Шевче́нко (25 лютого (9 березня) 1814, с. Моринці, Київська губернія, Російська імперія (нині Звенигородський район, Черкаська область, Україна) — 26 лютого (10 березня) 1861, Санкт-Петербург, Російська імперія) — український поет, прозаїк, мислитель, живописець, гравер, етнограф, громадський діяч. Національний герой і символ України. Діяч українського національного руху, член Кирило-Мефодіївського братства. Академік Імператорської академії мистецтв", "streetcode_person" },
                    { 2, new DateTime(2022, 12, 12, 20, 18, 50, 642, DateTimeKind.Local).AddTicks(7767), new DateTime(1885, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1817, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Мико́ла", 2, "Костома́ров", "Іва́нович", "Мико́ла Іва́нович Костома́ров (4 (16) травня 1817, с. Юрасівка, Острогозький повіт, Воронезька губернія — 7 (19) квітня 1885, Петербург) — видатний український[8][9][10][11][12] історик, етнограф, прозаїк, поет-романтик, мислитель, громадський діяч, етнопсихолог[13][14][15]. \r\n\r\nБув співзасновником та активним учасником слов'янофільсько-українського київського об'єднання «Кирило - Мефодіївське братство». У 1847 році за участь в українофільському братстві Костомарова арештовують та перевозять з Києва до Петербурга,де він і провів решту свого життя.", "streetcode_person" },
                    { 3, new DateTime(2022, 12, 12, 20, 18, 50, 642, DateTimeKind.Local).AddTicks(7774), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Василь", 3, "Білозерський", "Михайлович", "Білозерський Василь Михайлович (1825, хутір Мотронівка, Чернігівщина — 20 лютого (4 березня) 1899) — український громадсько-політичний і культурний діяч, журналіст.", "streetcode_person" }
                });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "subtitles",
                columns: new[] { "Id", "Description", "FirstName", "LastName", "Status", "StreetcodeId", "Url" },
                values: new object[,]
                {
                    { 8, null, "Nazarii", "Hovdysh", (byte)0, 4, null },
                    { 9, null, "Tatiana", "Shumylo", (byte)1, 4, null }
                });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "tags",
                columns: new[] { "Id", "Title" },
                values: new object[] { 4, "wictory" });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "texts",
                columns: new[] { "Id", "StreetcodeId", "TextContent", "Title" },
                values: new object[] { 4, 4, "Експерти пояснили, що дасть херсонська перемога українським силам\r\n\r\nНа тлі заяв окупантів про відведення військ та сил рф від Херсона та просування ЗСУ на херсонському напрямку українські бійці можуть отримати вогневий контроль над найважливішими дорогами Криму. Більше того, звільнення облцентру переріже постачання зброї для росії.", "визволення Херсона" });

            migrationBuilder.InsertData(
                schema: "toponyms",
                table: "toponyms",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { 3, null, "місто Херсон" });

            migrationBuilder.InsertData(
                schema: "transactions",
                table: "transaction_links",
                columns: new[] { "Id", "QrCodeUrl", "StreetcodeId", "Url" },
                values: new object[] { 4, "https://qrcode/4", 4, "https://streetcode/4" });

            migrationBuilder.InsertData(
                schema: "media",
                table: "videos",
                columns: new[] { "Id", "Description", "StreetcodeId", "Title", "Url" },
                values: new object[] { 4, "За виконанням Богдана Ступки", 4, "Вірш: Мені Однаково", "https://youtu.be/v3siIQi4nCQ" });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "coordinates",
                columns: new[] { "Id", "Latitude", "Longtitude", "StreetcodeId", "coordinate_type" },
                values: new object[] { 9, 50.4690m, 30.5328m, 3, "coordinate_streetcode" });

            migrationBuilder.InsertData(
                schema: "add_content",
                table: "coordinates",
                columns: new[] { "Id", "Latitude", "Longtitude", "ToponymId", "coordinate_type" },
                values: new object[] { 8, 46.3950m, 32.3738m, 3, "coordinate_toponym" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "media",
                table: "audios",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "coordinates",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "coordinates",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "coordinates",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "sources",
                table: "source_links",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "partner_sponsors",
                table: "streetcode_partners",
                keyColumns: new[] { "PartnerId", "StreetcodeId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                schema: "partner_sponsors",
                table: "streetcode_partners",
                keyColumns: new[] { "PartnerId", "StreetcodeId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "subtitles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "add_content",
                table: "tags",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "texts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "transactions",
                table: "transaction_links",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "media",
                table: "videos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "toponyms",
                table: "toponyms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "MiddleName",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreateDate", "EventEndOrPersonDeathDate", "EventStartOrPersonBirthDate", "Index", "Teaser", "Title", "ViewCount" },
                values: new object[] { new DateTime(2022, 12, 12, 18, 22, 32, 554, DateTimeKind.Local).AddTicks(9691), new DateTime(1861, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1814, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Тара́с Григо́рович Шевче́нко (25 лютого (9 березня) 1814, с. Моринці, Київська губернія, Російська імперія (нині Звенигородський район, Черкаська область, Україна) — 26 лютого (10 березня) 1861, Санкт-Петербург, Російська імперія) — український поет, прозаїк, мислитель, живописець, гравер, етнограф, громадський діяч. Національний герой і символ України. Діяч українського національного руху, член Кирило-Мефодіївського братства. Академік Імператорської академії мистецтв", "Some title", 0 });

            migrationBuilder.InsertData(
                schema: "streetcode",
                table: "streetcodes",
                columns: new[] { "Id", "CreateDate", "EventEndOrPersonDeathDate", "EventStartOrPersonBirthDate", "Index", "Teaser", "UpdateDate", "ViewCount", "streetcode_type" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 12, 18, 22, 32, 554, DateTimeKind.Local).AddTicks(9612), new DateTime(1861, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1814, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Тара́с Григо́рович Шевче́нко (25 лютого (9 березня) 1814, с. Моринці, Київська губернія, Російська імперія (нині Звенигородський район, Черкаська область, Україна) — 26 лютого (10 березня) 1861, Санкт-Петербург, Російська імперія) — український поет, прозаїк, мислитель, живописець, гравер, етнограф, громадський діяч. Національний герой і символ України. Діяч українського національного руху, член Кирило-Мефодіївського братства. Академік Імператорської академії мистецтв", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "streetcode_base" },
                    { 2, new DateTime(2022, 12, 12, 18, 22, 32, 554, DateTimeKind.Local).AddTicks(9649), new DateTime(1885, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1817, 5, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Мико́ла Іва́нович Костома́ров (4 (16) травня 1817, с. Юрасівка, Острогозький повіт, Воронезька губернія — 7 (19) квітня 1885, Петербург) — видатний український[8][9][10][11][12] історик, етнограф, прозаїк, поет-романтик, мислитель, громадський діяч, етнопсихолог[13][14][15]. \r\n\r\nБув співзасновником та активним учасником слов'янофільсько-українського київського об'єднання «Кирило - Мефодіївське братство». У 1847 році за участь в українофільському братстві Костомарова арештовують та перевозять з Києва до Петербурга,де він і провів решту свого життя.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "streetcode_base" },
                    { 3, new DateTime(2022, 12, 12, 18, 22, 32, 554, DateTimeKind.Local).AddTicks(9652), new DateTime(1899, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1825, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Білозерський Василь Михайлович (1825, хутір Мотронівка, Чернігівщина — 20 лютого (4 березня) 1899) — український громадсько-політичний і культурний діяч, журналіст.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "streetcode_base" }
                });
        }
    }
}
