using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class AddAlias_DateString_UpdateTitle_Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateString",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "streetcode",
                table: "streetcodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 6,
                column: "Url",
                value: "https://i.ibb.co/RB9KtSq/Ukrainka.png");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "DateString" },
                values: new object[] { new DateTime(2023, 3, 3, 19, 26, 34, 442, DateTimeKind.Local).AddTicks(5828), "11 листопада 2022" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Alias", "CreatedAt", "DateString", "Title" },
                values: new object[] { "Кобзар", new DateTime(2023, 3, 3, 19, 26, 34, 442, DateTimeKind.Local).AddTicks(5527), "9 березня 1814 — 10 березня 1861", "Тарас Шевченко" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "DateString", "Title" },
                values: new object[] { new DateTime(2023, 3, 3, 19, 26, 34, 442, DateTimeKind.Local).AddTicks(5745), "9 березня 1814 — 10 березня 1861", "Мико́ла Костома́ров" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "DateString", "Title" },
                values: new object[] { new DateTime(2023, 3, 3, 19, 26, 34, 442, DateTimeKind.Local).AddTicks(5750), "2 січня 1825 — 20 лютого 1899", "Василь Білозерський" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "DateString", "Title" },
                values: new object[] { new DateTime(2023, 3, 3, 19, 26, 34, 442, DateTimeKind.Local).AddTicks(5763), "2 січня 1825 — 20 лютого 1899", "Володимир-Варфоломей Кропивницький-Шевченківський" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Alias", "CreatedAt", "DateString", "Title" },
                values: new object[] { "Лариса Косач", new DateTime(2023, 3, 3, 19, 26, 34, 442, DateTimeKind.Local).AddTicks(5768), "2 січня 1825 — 20 лютого 1899", "Леся Українка" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "DateString", "Title" },
                values: new object[] { new DateTime(2023, 3, 3, 19, 26, 34, 442, DateTimeKind.Local).AddTicks(5772), "2 січня 1825 — 20 лютого 1899", "Іван Мазепа" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "texts",
                keyColumn: "Id",
                keyValue: 1,
                column: "TextContent",
                value: "Тарас Шевченко народився 9 березня 1814 року в селі Моринці Пединівської волості Звенигородського повіту Київської губернії. Був третьою дитиною селян-кріпаків Григорія Івановича Шевченка та Катерини Якимівни після сестри Катерини (1804 — близько 1848) та брата Микити (1811 — близько 1870).\n\n                    За родинними переказами, Тарасові діди й прадіди з батьківського боку походили від козака Андрія, який на початку XVIII століття прийшов із Запорізької Січі. Батьки його матері, Катерини Якимівни Бойко, були переселенцями з Прикарпаття.\n\n                    1816 року сім'я Шевченків переїхала до села Кирилівка Звенигородського повіту, звідки походив Григорій Іванович. Дитячі роки Тараса пройшли в цьому селі. 1816 року народилася Тарасова сестра Ярина, 1819 року — сестра Марія, а 1821 року народився Тарасів брат Йосип.\n\n                    Восени 1822 року Тарас Шевченко почав учитися грамоти у дяка Совгиря. Тоді ж ознайомився з творами Григорія Сковороди.\n\n                    10 лютого 1823 року його старша сестра Катерина вийшла заміж за Антона Красицького — селянина із Зеленої Діброви, а 1 вересня 1823 року від тяжкої праці й злиднів померла мати Катерина. \n\n                    19 жовтня 1823 року батько одружився вдруге з удовою Оксаною Терещенко, в якої вже було троє дітей. Вона жорстоко поводилася з нерідними дітьми, зокрема з малим Тарасом. 1824 року народилася Тарасова сестра Марія — від другого батькового шлюбу.\n\n                    Хлопець чумакував із батьком. Бував у Звенигородці, Умані, Єлисаветграді (тепер Кропивницький). 21 березня (2 квітня) 1825 року батько помер, і невдовзі мачуха повернулася зі своїми трьома дітьми до Моринців. Зрештою Тарас змушений був залишити домівку. Деякий час Тарас жив у свого дядька Павла, який після смерті його батька став опікуном сиріт. Дядько Павло був «великий катюга»; Тарас працював у нього, разом із наймитом у господарстві, але у підсумку не витримав тяжких умов життя й пішов у найми до нового кирилівського дяка Петра Богорського.\n\n                    Як попихач носив воду, опалював школу, обслуговував дяка, читав псалтир над померлими і вчився. Не стерпівши знущань Богорського й відчуваючи великий потяг до живопису, Тарас утік від дяка й почав шукати в навколишніх селах учителя-маляра. Кілька днів наймитував і «вчився» малярства в диякона Єфрема. Також мав учителів-малярів із села Стеблева, Канівського повіту та із села Тарасівки Звенигородського повіту. 1827 року він пас громадську отару в Кирилівці й там зустрічався з Оксаною Коваленко. Згодом подругу свого дитинства поет не раз згадає у своїх творах і присвятить їй поему «Мар'яна-черниця».");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.DropColumn(
                name: "DateString",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "streetcode",
                table: "streetcodes");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "streetcode",
                table: "streetcodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 6,
                column: "Url",
                value: "https://i.ibb.co/f85t1Vs/Antonovich.png");

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6954));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6882));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6917));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6922));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6927));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6931));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 2, 1, 17, 42, 51, 482, DateTimeKind.Local).AddTicks(6935));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "texts",
                keyColumn: "Id",
                keyValue: 1,
                column: "TextContent",
                value: "Тарас Шевченко народився 9 березня 1814 року в селі Моринці Пединівської волості Звенигородського повіту Київської губернії. Був третьою дитиною селян-кріпаків Григорія Івановича Шевченка та Катерини Якимівни після сестри Катерини (1804 — близько 1848) та брата Микити (1811 — близько 1870).\r\n\r\n                    За родинними переказами, Тарасові діди й прадіди з батьківського боку походили від козака Андрія, який на початку XVIII століття прийшов із Запорізької Січі. Батьки його матері, Катерини Якимівни Бойко, були переселенцями з Прикарпаття.\r\n\r\n                    1816 року сім'я Шевченків переїхала до села Кирилівка Звенигородського повіту, звідки походив Григорій Іванович. Дитячі роки Тараса пройшли в цьому селі. 1816 року народилася Тарасова сестра Ярина, 1819 року — сестра Марія, а 1821 року народився Тарасів брат Йосип.\r\n\r\n                    Восени 1822 року Тарас Шевченко почав учитися грамоти у дяка Совгиря. Тоді ж ознайомився з творами Григорія Сковороди.\r\n\r\n                    10 лютого 1823 року його старша сестра Катерина вийшла заміж за Антона Красицького — селянина із Зеленої Діброви, а 1 вересня 1823 року від тяжкої праці й злиднів померла мати Катерина. \r\n\r\n                    19 жовтня 1823 року батько одружився вдруге з удовою Оксаною Терещенко, в якої вже було троє дітей. Вона жорстоко поводилася з нерідними дітьми, зокрема з малим Тарасом. 1824 року народилася Тарасова сестра Марія — від другого батькового шлюбу.\r\n\r\n                    Хлопець чумакував із батьком. Бував у Звенигородці, Умані, Єлисаветграді (тепер Кропивницький). 21 березня (2 квітня) 1825 року батько помер, і невдовзі мачуха повернулася зі своїми трьома дітьми до Моринців. Зрештою Тарас змушений був залишити домівку. Деякий час Тарас жив у свого дядька Павла, який після смерті його батька став опікуном сиріт. Дядько Павло був «великий катюга»; Тарас працював у нього, разом із наймитом у господарстві, але у підсумку не витримав тяжких умов життя й пішов у найми до нового кирилівського дяка Петра Богорського.\r\n\r\n                    Як попихач носив воду, опалював школу, обслуговував дяка, читав псалтир над померлими і вчився. Не стерпівши знущань Богорського й відчуваючи великий потяг до живопису, Тарас утік від дяка й почав шукати в навколишніх селах учителя-маляра. Кілька днів наймитував і «вчився» малярства в диякона Єфрема. Також мав учителів-малярів із села Стеблева, Канівського повіту та із села Тарасівки Звенигородського повіту. 1827 року він пас громадську отару в Кирилівці й там зустрічався з Оксаною Коваленко. Згодом подругу свого дитинства поет не раз згадає у своїх творах і присвятить їй поему «Мар'яна-черниця».");
        }
    }
}
