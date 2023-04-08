using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Streetcode.DAL.Persistence.Migrations
{
    public partial class ChangeAudioandImagetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "media",
                table: "images",
                newName: "MimeType");

            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "media",
                table: "audios",
                newName: "MimeType");

            migrationBuilder.AddColumn<string>(
                name: "BlobStorageName",
                schema: "media",
                table: "images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BlobStorageName",
                schema: "media",
                table: "audios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "audios",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://somelink1", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "audios",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://somelink2", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "audios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://somelink3", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "audios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://somelink4", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "http://www.univ.kiev.ua/tpl/img/photo-osobystosti/foto-shevchenko.jpg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/commons/1/10/Taras_Shevchenko_painting_0001.jpg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Enhelhard_by_Shevchenko.jpg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Portret_nevidomoho_Shevchenko_.jpg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://www.megakniga.com.ua/uploads/cache/Products/Product_images_343456/d067b1_w1600.jpg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/commons/2/21/PGRS_2_051_Kostomarov_-_crop.jpg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/commons/6/6a/%D0%91%D0%B5%D0%BB%D0%BE%D0%B7%D0%B5%D1%80%D1%81%D0%BA%D0%B8%D0%B9_%D0%92%D0%B0%D1%81%D0%B8%D0%BB%D0%B8%D0%B9.JPG", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://img.tsn.ua/cached/907/tsn-15890496c3fba55a55e21f0ca3090d06/thumbs/x/3e/1a/97fe20f34f78c6f13ea84dbf15ee1a3e.jpeg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://images.unsplash.com/photo-1589998059171-988d887df646?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8b3BlbiUyMGJvb2t8ZW58MHx8MHx8&w=1000&q=80", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://www.earnmydegree.com/sites/all/files/public/video-prod-image.jpg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://images.laws.com/constitution/constitutional-convention.jpg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://itukraine.org.ua/files/img/illus/members/softserve%20logo.png", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://static.ua-football.com/img/upload/19/270071.png", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://communitypartnersinc.org/wp-content/uploads/2018/03/CP_Logo_RGB_Horizontal-e1520810390513.png", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2d/Ecumenical_Patriarch_Bartholomew_in_the_Vatican_2021_%28cropped%29.jpg/800px-Ecumenical_Patriarch_Bartholomew_in_the_Vatican_2021_%28cropped%29.jpg", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://api.culture.pl/sites/default/files/styles/embed_image_360/public/2022-03/lesya_ukrainka_portrait_public_domain.jpg?itok=1jAIv48D", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "https://reibert.info/attachments/hetmans_catalog-1-4-scaled-jpg.18981447/", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "/assets/2296e9b1db2ab72f2db9.png", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "/assets/35b44f042d027c3a7589.png", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "/assets/c58dac51751395fb3217.png", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "/assets/233c6bbb0b79df230d93.png", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "/assets/02b59f4ef917107514e3.png", "" });

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "BlobStorageName", "MimeType" },
                values: new object[] { "/assets/8ecaa9756bac938f8f73.png", "" });

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8483));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8420));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8452));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8456));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8459));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8463));

            migrationBuilder.UpdateData(
                schema: "streetcode",
                table: "streetcodes",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2023, 3, 17, 16, 48, 43, 643, DateTimeKind.Local).AddTicks(8466));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobStorageName",
                schema: "media",
                table: "images");

            migrationBuilder.DropColumn(
                name: "BlobStorageName",
                schema: "media",
                table: "audios");

            migrationBuilder.RenameColumn(
                name: "MimeType",
                schema: "media",
                table: "images",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "MimeType",
                schema: "media",
                table: "audios",
                newName: "Url");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "audios",
                keyColumn: "Id",
                keyValue: 1,
                column: "Url",
                value: "https://somelink1");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "audios",
                keyColumn: "Id",
                keyValue: 2,
                column: "Url",
                value: "https://somelink2");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "audios",
                keyColumn: "Id",
                keyValue: 3,
                column: "Url",
                value: "https://somelink3");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "audios",
                keyColumn: "Id",
                keyValue: 4,
                column: "Url",
                value: "https://somelink4");

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
                keyValue: 2,
                column: "Url",
                value: "https://upload.wikimedia.org/wikipedia/commons/1/10/Taras_Shevchenko_painting_0001.jpg");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 3,
                column: "Url",
                value: "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Enhelhard_by_Shevchenko.jpg");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 4,
                column: "Url",
                value: "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Portret_nevidomoho_Shevchenko_.jpg");

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
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 9,
                column: "Url",
                value: "https://images.unsplash.com/photo-1589998059171-988d887df646?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY2h8Mnx8b3BlbiUyMGJvb2t8ZW58MHx8MHx8&w=1000&q=80");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 10,
                column: "Url",
                value: "https://www.earnmydegree.com/sites/all/files/public/video-prod-image.jpg");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 11,
                column: "Url",
                value: "https://images.laws.com/constitution/constitutional-convention.jpg");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 12,
                column: "Url",
                value: "https://itukraine.org.ua/files/img/illus/members/softserve%20logo.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 13,
                column: "Url",
                value: "https://static.ua-football.com/img/upload/19/270071.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 14,
                column: "Url",
                value: "https://communitypartnersinc.org/wp-content/uploads/2018/03/CP_Logo_RGB_Horizontal-e1520810390513.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 15,
                column: "Url",
                value: "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2d/Ecumenical_Patriarch_Bartholomew_in_the_Vatican_2021_%28cropped%29.jpg/800px-Ecumenical_Patriarch_Bartholomew_in_the_Vatican_2021_%28cropped%29.jpg");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 16,
                column: "Url",
                value: "https://api.culture.pl/sites/default/files/styles/embed_image_360/public/2022-03/lesya_ukrainka_portrait_public_domain.jpg?itok=1jAIv48D");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 17,
                column: "Url",
                value: "https://reibert.info/attachments/hetmans_catalog-1-4-scaled-jpg.18981447/");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 18,
                column: "Url",
                value: "/assets/2296e9b1db2ab72f2db9.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 19,
                column: "Url",
                value: "/assets/35b44f042d027c3a7589.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 20,
                column: "Url",
                value: "/assets/c58dac51751395fb3217.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 21,
                column: "Url",
                value: "/assets/233c6bbb0b79df230d93.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 22,
                column: "Url",
                value: "/assets/02b59f4ef917107514e3.png");

            migrationBuilder.UpdateData(
                schema: "media",
                table: "images",
                keyColumn: "Id",
                keyValue: 23,
                column: "Url",
                value: "/assets/8ecaa9756bac938f8f73.png");

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
        }
    }
}
