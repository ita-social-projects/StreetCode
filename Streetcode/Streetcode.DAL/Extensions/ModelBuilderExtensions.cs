using Microsoft.EntityFrameworkCore;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Sources;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.Types;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Enums;

namespace Streetcode.DAL.Extensions;

public static class ModelBuilderExtensions
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subtitle>().HasData(
            new Subtitle
            {
                Id = 1,
                SubtitleText = "some text",
                StreetcodeId = 1
            },
            new Subtitle
            {
                Id = 2,
                SubtitleText = "some text",
                StreetcodeId = 2
            },
            new Subtitle
            {
                Id = 3,
                SubtitleText = "some text",
                StreetcodeId = 3
            },
            new Subtitle
            {
                Id = 4,
                SubtitleText = "some text",
                StreetcodeId = 1
            },
            new Subtitle
            {
                Id = 5,
                SubtitleText = "some text",
                StreetcodeId = 2
            },
            new Subtitle
            {
                Id = 6,
                SubtitleText = "some text",
                StreetcodeId = 1
            },
            new Subtitle
            {
                Id = 7,
                SubtitleText = "some text",
                StreetcodeId = 3
            },
            new Subtitle
            {
                Id = 8,
                SubtitleText = "some text",
                StreetcodeId = 4
            },
            new Subtitle
            {
                Id = 9,
                SubtitleText = "some text",
                StreetcodeId = 4
            });
        modelBuilder.Entity<Tag>().HasData(
            new Tag
            {
                Id = 1,
                Title = "writer"
            },
            new Tag
            {
                Id = 2,
                Title = "artist"
            },
            new Tag
            {
                Id = 3,
                Title = "composer"
            },
            new Tag
            {
                Id = 4,
                Title = "wictory"
            },
            new Tag
            {
                Id = 5,
                Title = "Наукова школа"
            },
            new Tag
            {
                Id = 6,
                Title = "Історія"
            },
            new Tag
            {
                Id = 7,
                Title = "Політика"
            });
        modelBuilder.Entity<Response>().HasData(
            new Response
            {
                Id = 1,
                Description = "Good Job",
                Email = "dmytrobuchkovsky@gmail.com"
            },
            new Response
            {
                Id = 2,
                Name = "Dmytro",
                Description = "Nice project",
                Email = "mail@gmail.com"
            });
        modelBuilder.Entity<Art>().HasData(
            new Art
            {
                Id = 1,
                ImageId = 18,
                Description = "Анатолій Федірко, «Український супрематичний політичний діяч Михайло Грушевський», 2019-2020 роки."
            },
            new Art
            {
                Id = 2,
                ImageId = 19,
            },
            new Art
            {
                Id = 3,
                ImageId = 20,
                Description = "Назар Дубів опублікував серію малюнків, у яких перетворив класиків української літератури та політичних діячів на сучасних модників"
            },
            new Art
            {
                Id = 4,
                ImageId = 21
            },
            new Art
            {
                Id = 5,
                ImageId = 22,
                Description = "«Козаки на орбіті» поєднує не тільки тему козаків, а й апелює до космічної тематики."
            },
            new Art
            {
                Id = 6,
                ImageId = 23,
                Description = "На вулиці Січових стрільців, 75 закінчили малювати мурал Михайла Грушевського на місці малюнка будинку з лелекою."
            });
        modelBuilder.Entity<Image>().HasData(
                        new
                        {
                            Id = 1,
                            Alt = "Портрет Тараса Шевченка",
                            BlobName = "ea97sDiJCukzoLUoYZhE4fyOPxsJQeFCgmI9Ub3_iVw=.gif",
                            MimeType = "image/gif",
                            Title = "Тарас Шевченко"
                        },
                        new
                        {
                            Id = 2,
                            Alt = "Тарас Шевченко: Погруддя жінки",
                            BlobName = "LS0Ag4HI4nu+EWFsPgNg5a+1B08kWdcOWw2sMbpimVk=.jpeg",
                            MimeType = "image/jpeg",
                            Title = "Погруддя жінки"
                        },
                        new
                        {
                            Id = 3,
                            Alt = "Тарас Шевченко: Портрет Павла Васильовича Енгельгардта",
                            BlobName = "okhbqu+U3+ErGdOK0RA7V5yF80BaDOh+hBadZ0zMI3w=.jpeg",
                            MimeType = "image/jpeg",
                            Title = "Портрет Павла Васильовича Енгельгардта"
                        },
                        new
                        {
                            Id = 4,
                            Alt = "Тарас Шевченко: Портрет невідомого",
                            BlobName = "VXvgD9+F+iuYCHIn4pZrRyPa+ljB2XHepOJAeUIy39g=.jpeg",
                            MimeType = "image/jpeg",
                            Title = "Портрет невідомого"
                        },
                        new
                        {
                            Id = 5,
                            Alt = "Кобзар",
                            BlobName = "ED_kNMjZkMDz6_syM5klb8HGDyfU72Q6Sdz_Y4DmCJ8=.png",
                            MimeType = "image/png",
                            Title = "Кобзар"
                        },
                        new
                        {
                            Id = 6,
                            Alt = "Мико́ла Костома́ров",
                            BlobName = "VryHfNFydefxGWzL9Q2TFnykRwBvBjb7g7StwOztV+k=.png",
                            MimeType = "image/png",
                            Title = "Мико́ла Костома́ров"
                        },
                        new
                        {
                            Id = 7,
                            Alt = "Василь Білозерський",
                            BlobName = "LOJwfRkqVqkjchzf9cLnSsFmTXdYaLUio+iRaDA3vVE=.png",
                            MimeType = "image/png",
                            Title = "Василь Білозерський"
                        },
                        new
                        {
                            Id = 8,
                            Alt = "Звільнення Херсона",
                            BlobName = "cxU2wbUcQWQMrnC5bMUG4u8am9UIu2yFko+wPg9UBRM=.png",
                            MimeType = "image/png",
                            Title = "Звільнення Херсона"
                        },
                        new
                        {
                            Id = 9,
                            Alt = "book",
                            BlobName = "i0OXuaqrvuGioxRAD045vizgs6S7rluDPUMuyYm3YyY=.png",
                            MimeType = "image/png",
                            Title = "book"
                        },
                        new
                        {
                            Id = 10,
                            Alt = "video",
                            BlobName = "6BUOLcI9EwKDZTDi6ZPJyVnU5SaGOYSijBjdQH+4oUU=.png",
                            MimeType = "image/png",
                            Title = "video"
                        },
                        new
                        {
                            Id = 11,
                            Alt = "article",
                            BlobName = "eQVD_pSkN0maCTFVPAjtX0QvJmsur7VIL9ZYvGAdIQ8=.png",
                            MimeType = "image/png",
                            Title = "article"
                        },
                        new
                        {
                            Id = 12,
                            Alt = "SoftServe",
                            BlobName = "4w2OMhGbkdKizWWAUsMBfCygW0HMhLcvwVtvGrZtdhc=.png",
                            MimeType = "image/png",
                            Title = "SoftServe"
                        },
                        new
                        {
                            Id = 13,
                            Alt = "Parimatch",
                            BlobName = "t3DupxlKv0+h0tYIjUMgnzn7BXxBcYzj1WAdBUBlkjs=.png",
                            MimeType = "image/png",
                            Title = "Parimatch"
                        },
                        new
                        {
                            Id = 14,
                            Alt = "Community Partners",
                            BlobName = "nfuYLRSVuaGDTGjKLuerynSNVtyI3npHR+q65KNanKQ=.png",
                            MimeType = "image/png",
                            Title = "Community Partners"
                        },
                        new
                        {
                            Id = 15,
                            Alt = "Володимир-Варфоломей",
                            BlobName = "88VCISdBuZeWwIZSmnVIXsHUGQzFMTh4iPGEbU1+ckQ=.png",
                            MimeType = "image/png",
                            Title = "Володимир-Варфоломей"
                        },
                        new
                        {
                            Id = 16,
                            Alt = "Леся Українка",
                            BlobName = "U2Sn_n0vXTwdCBabM2XzFPhxLXrff8QoomoLL4xGadc=.png",
                            MimeType = "image/png",
                            Title = "Леся Українка"
                        },
                        new
                        {
                            Id = 17,
                            Alt = "Іван Мазепа",
                            BlobName = "bnIzOC+dFAYy9CtrojbMjl3RlEWIdGIx2TJeSZWC97c=.png",
                            MimeType = "image/png",
                            Title = "Іван Мазепа"
                        },
                        new
                        {
                            Id = 18,
                            Alt = "Грушевький",
                            BlobName = "eke98jD1VRreBj_lOPQznFFZujua_+esR1R7vHgRk_g=.png",
                            MimeType = "image/png",
                            Title = "Михайло Грушевський"
                        },
                        new
                        {
                            Id = 19,
                            Alt = "Грушевький",
                            BlobName = "vk9ydYvn4hjSlRuW+K9XDwJ68n+7GLx0tz6djJCj__Q=.png",
                            MimeType = "image/png",
                            Title = "Грушевський"
                        },
                        new
                        {
                            Id = 20,
                            Alt = "Грушевський",
                            BlobName = "8Xw3NUnskB1sDHjSV4tAX74tO+zF1dPQMZ287e3GQbg=.png",
                            MimeType = "image/png",
                            Title = "Сучасний Грушевський"
                        },
                        new
                        {
                            Id = 21,
                            Alt = "мурал",
                            BlobName = "vk9ydYvn4hjSlRuW+K9XDwJ68n+7GLx0tz6djJCj__Q=.png",
                            MimeType = "image/png",
                            Title = "Мурал Грушевського"
                        },
                        new
                        {
                            Id = 22,
                            Alt = "Козаки на орбіті",
                            BlobName = "QdH_sDDsthyCnLA_X4kfIo9PzwR5_MKTPMJbsujA3T4=.png",
                            MimeType = "image/png",
                            Title = "Козаки на орбіті"
                        },
                        new
                        {
                            Id = 23,
                            Alt = "мурал",
                            BlobName = "QdH_sDDsthyCnLA_X4kfIo9PzwR5_MKTPMJbsujA3T4=.png",
                            MimeType = "image/png",
                            Title = "Мурал М. Грушевського"
                        });
        modelBuilder.Entity<Audio>().HasData(
            new Audio
            {
                Id = 1,
                Title = "audio1",
                Description = "for streetcode1",
                BlobName = "tCK3PO79PB2mT_HbQAtlqfHnL0N8mHu2el_vZF2uj0g=.mp3",
                MimeType = "audio/mpeg",
                StreetcodeId = 1
            },
            new Audio
            {
                Id = 2,
                Title = "audio2",
                Description = "for streetcode2",
                BlobName = "tCK3PO79PB2mT_HbQAtlqfHnL0N8mHu2el_vZF2uj0g=.mp3",
                MimeType = "audio/mpeg",
                StreetcodeId = 2
            },
            new Audio
            {
                Id = 3,
                Title = "audio3",
                Description = "for streetcode3",
                BlobName = "tCK3PO79PB2mT_HbQAtlqfHnL0N8mHu2el_vZF2uj0g=.mp3",
                MimeType = "audio/mpeg",
                StreetcodeId = 3
            },
            new Audio
            {
                Id = 4,
                Title = "audio4",
                Description = "for streetcode4",
                BlobName = "tCK3PO79PB2mT_HbQAtlqfHnL0N8mHu2el_vZF2uj0g=.mp3",
                MimeType = "audio/mpeg",
                StreetcodeId = 4
            });
        modelBuilder.Entity<StreetcodeCoordinate>().HasData(
            new StreetcodeCoordinate
            {
                Id = 6,
                Latitude = 49.8429M,
                Longtitude = 24.0311M,
                StreetcodeId = 1
            },
            new StreetcodeCoordinate
            {
                Id = 7,
                Latitude = 50.4550M,
                Longtitude = 30.5238M,
                StreetcodeId = 2
            },
            new StreetcodeCoordinate
            {
                Id = 9,
                Latitude = 50.4690M,
                Longtitude = 30.5328M,
                StreetcodeId = 3
            },
            new StreetcodeCoordinate
            {
                Id = 10,
                Latitude = 46.3950M,
                Longtitude = 32.3738M,
                StreetcodeId = 4
            });
        modelBuilder.Entity<HistoricalContext>().HasData(
            new HistoricalContext
            {
                Id = 1,
                Title = "Дитинство"
            },
            new HistoricalContext
            {
                Id = 2,
                Title = "Студентство"
            },
            new HistoricalContext
            {
                Id = 3,
                Title = "Життя в Петербурзі"
            });
        modelBuilder.Entity<Video>().HasData(
            new Video
            {
                Id = 1,
                Title = "audio1",
                Description = "for streetcode1",
                Url = "https://somelink1",
                StreetcodeId = 2
            },
            new Video
            {
                Id = 2,
                Title = "Біографія Т.Г.Шевченка",
                Url = "https://www.youtube.com/watch?v=VVFEi6lTpZk&ab_channel=%D0%9E%D1%81%D1%82%D0%B0%D0%BD%D0%BD%D1%96%D0%B9%D0%93%D0%B5%D1%82%D1%8C%D0%BC%D0%B0%D0%BD",
                StreetcodeId = 1
            },
            new Video
            {
                Id = 3,
                Title = "Вірш: Мені Однаково",
                Description = "За виконанням Богдана Ступки",
                Url = "https://www.youtube.com/watch?v=f55dHPEY-0U&ab_channel=%D0%86%D0%B3%D0%BE%D1%80%D0%9E%D0%BF%D0%B0%D1%86%D1%8C%D0%BA%D0%B8%D0%B9",
                StreetcodeId = 1
            },
            new Video
            {
                Id = 4,
                Title = "Вірш: Мені Однаково",
                Description = "За виконанням Богдана Ступки",
                Url = "https://youtu.be/v3siIQi4nCQ",
                StreetcodeId = 4
            });
        modelBuilder.Entity<Partner>().HasData(
            new Partner
            {
                Id = 1,
                IsKeyPartner = true,
                Title = "SoftServe",
                Description = "Український культурний фонд є флагманською українською інституцією культури, яка у своїй діяльності інтегрує" +
                    " різні види мистецтва – від сучасного мистецтва, нової музики й театру до літератури та музейної справи." +
                    " Мистецький арсенал є флагманською українською інституцією культури, яка у своїй діяльності інтегрує різні" +
                    " види мистецтва – від сучасного мистецтва, нової музики й театру до літератури та музейної справи.",
                LogoId = 12,
                TargetUrl = "https://www.softserveinc.com/en-us",
                UrlTitle = "go to SoftServe page"
            },
            new Partner
            {
                Id = 2,
                Title = "Parimatch",
                Description = "some text",
                LogoId = 13,
                TargetUrl = "https://parimatch.com/"
            },
            new Partner
            {
                Id = 3,
                Title = "comunity partner",
                LogoId = 14,
                TargetUrl = "https://partners.salesforce.com/pdx/s/?language=en_US&redirected=RGSUDODQUL"
            });
        modelBuilder.Entity<PartnerSourceLink>().HasData(
            new PartnerSourceLink
            {
                Id = 1,
                Title = "Twitter",
                LogoType = LogoType.Twitter,
                TargetUrl = "https://twitter.com/SoftServeInc",
                PartnerId = 1
            },
            new PartnerSourceLink
            {
                Id = 2,
                Title = "Instagram",
                LogoType = LogoType.Instagram,
                TargetUrl = "https://www.instagram.com/softserve_people/",
                PartnerId = 1
            },
            new PartnerSourceLink
            {
                Id = 3,
                Title = "facebook",
                LogoType = LogoType.Facebook,
                TargetUrl = "https://www.facebook.com/SoftServeCompany",
                PartnerId = 1
            });
        modelBuilder.Entity<StreetcodeArt>().HasData(
            new StreetcodeArt
            {
                ArtId = 1,
                StreetcodeId = 1,
                Index = 1,
            },
            new StreetcodeArt
            {
                ArtId = 2,
                StreetcodeId = 1,
                Index = 2,
            },
            new StreetcodeArt
            {
                ArtId = 3,
                StreetcodeId = 1,
                Index = 3,
            },
            new StreetcodeArt
            {
                ArtId = 4,
                StreetcodeId = 1,
                Index = 4,
            },
            new StreetcodeArt
            {
                ArtId = 5,
                StreetcodeId = 1,
                Index = 5,
            },
            new StreetcodeArt
            {
                ArtId = 6,
                StreetcodeId = 1,
                Index = 6,
            });
        modelBuilder.Entity<SourceLink>().HasData(
            new SourceLink
            {
                Id = 1,
                Title = "Том 2: Суспільно-політичні твори (1907–1914).",
                Url =
                    "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87",
            },
            new SourceLink
            {
                Id = 2,
                Title = "Том 3: Суспільно-політичні твори (1907 — березень 1917).",
                Url =
                    "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf",
            },
            new SourceLink
            {
                Id = 3,
                Title =
                    "Том 4. Книга 1: Суспільно-політичні твори (доба Української Центральної Ради, березень 1917 — квітень 1918).",
                Url =
                    "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html",
            },
            new SourceLink
            {
                Id = 4,
                Title = "Том 5: Історичні студії та розвідки (1888–1896).",
                Url =
                    "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87",
            },
            new SourceLink
            {
                Id = 5,
                Title = "Том 6: Історичні студії та розвідки (1895–1900).",
                Url =
                    "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf",
            },
            new SourceLink
            {
                Id = 6,
                Title = "Том 7: Історичні студії та розвідки (1900–1906).",
                Url =
                    "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html",
            },
            new SourceLink
            {
                Id = 7,
                Title = "Том 8: Історичні студії та розвідки (1906–1916).",
                Url =
                    "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87",
            },
            new SourceLink
            {
                Id = 8,
                Title = "Том 9: Історичні студії та розвідки (1917–1923).",
                Url =
                    "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf",
            },
            new SourceLink
            {
                Id = 9,
                Title = "Том 10. Книга 1: Історичні студії та розвідки (1924— 1930)/ упор. О.Юркова.",
                Url =
                    "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html",
            },
            new SourceLink
            {
                Id = 10,
                Title = "Том 10. Книга 2: Історичні студії та розвідки (1930— 1934)",
                Url =
                    "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html",
            },
            new SourceLink
            {
                Id = 11,
                Title = "Том 11: Літературно-критичні праці (1883–1931), «По світу»",
                Url =
                    "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87",
            },
            new SourceLink
            {
                Id = 12,
                Title = "Том 12: Поезія (1882–1903). Проза, драматичні твори, переклади (1883–1886)",
                Url =
                    "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87",
            },
            new SourceLink
            {
                Id = 13,
                Title = "Том 13 : Серія \"Літературно-критичні та художні твори (1887-1924)\"",
                Url =
                    "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf",
            },
            new SourceLink
            {
                Id = 14,
                Title = "Том 14: Рецензії та огляди (1888–1897).",
                Url =
                    "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html",
            });
        modelBuilder.Entity<SourceLinkCategory>().HasData(
            new SourceLinkCategory
            {
                Id = 1,
                Title = "Книги",
                ImageId = 9,
            },
            new SourceLinkCategory
            {
                Id = 2,
                Title = "Фільми",
                ImageId = 10,
            },
            new SourceLinkCategory
            {
                Id = 3,
                Title = "Цитати",
                ImageId = 11,
            });
        modelBuilder.Entity<SourceLinkSubCategory>().HasData(
            new SourceLinkSubCategory
            {
                Id = 1,
                Title = "Фільми про Т. Г. Шевченко",
                SourceLinkCategoryId = 2,
            },
            new SourceLinkSubCategory
            {
                Id = 2,
                Title = "Хроніки про Т. Г. Шевченко",
                SourceLinkCategoryId = 2,
            },
            new SourceLinkSubCategory
            {
                Id = 3,
                Title = "Блоги про Т. Г. Шевченко",
                SourceLinkCategoryId = 2,
            },
            new SourceLinkSubCategory
            {
                Id = 4,
                Title = "Праці Грушевського",
                SourceLinkCategoryId = 1,
            },
            new SourceLinkSubCategory
            {
                Id = 5,
                Title = "Книги про Грушевського",
                SourceLinkCategoryId = 1,
            },
            new SourceLinkSubCategory
            {
                Id = 6,
                Title = "Статті",
                SourceLinkCategoryId = 1,
            },
            new SourceLinkSubCategory
            {
                Id = 7,
                Title = "Пряма мова",
                SourceLinkCategoryId = 3,
            });
        modelBuilder.Entity<Fact>().HasData(
            new Fact
            {
                Id = 1,
                Title = "Викуп з кріпацтва",
                FactContent = "Навесні 1838-го Карл Брюллов і Василь Жуковський вирішили викупити молодого поета з кріпацтва. " +
                    "Енгельгардт погодився відпустити кріпака за великі гроші — 2500 рублів. Щоб здобути такі гроші, Карл Брюллов" +
                    " намалював портрет Василя Жуковського — вихователя спадкоємця престолу Олександра Миколайовича, і портрет розіграли" +
                    " в лотереї, у якій взяла участь імператорська родина. Лотерея відбулася 4 травня 1838 року," +
                    " а 7 травня Шевченкові видали відпускну.",
                ImageId = 6
            },
            new Fact
            {
                Id = 2,
                Title = "Перший Кобзар",
                FactContent = " Ознайомившись випадково з рукописними творами Шевченка й вражений ними, П. Мартос виявив до них великий інтерес." +
                    " Він порадився із Є. Гребінкою і запропонував Шевченку видати їх окремою книжкою, яку згодом назвали «Кобзарем».",
                ImageId = 5
            });
        modelBuilder.Entity<Term>().HasData(
            new Term
            {
                Id = 1,
                Title = "етнограф",
                Description = "Етнографія — суспільствознавча наука, об'єктом дослідження якої є народи, їхня культура і побут, походження, розселення," +
                    " процеси культурно-побутових відносин на всіх етапах історії людства."
            },
            new Term
            {
                Id = 2,
                Title = "гравер",
                Description = "Гра́фіка — вид образотворчого мистецтва, для якого характерна перевага ліній і штрихів, використання контрастів білого та" +
                    " чорного та менше, ніж у живописі, використання кольору. Твори можуть мати як монохромну, так і поліхромну гаму."
            },
            new Term
            {
                Id = 3,
                Title = "кріпак",
                Description = "Кріпа́цтво, або кріпосне́ право, у вузькому сенсі — правова система, або система правових норм при феодалізмі, яка встановлювала" +
                    " залежність селянина від феодала й неповну власність феодала на селянина."
            },
            new Term
            {
                Id = 4,
                Title = "мачуха",
                Description = "Ма́чуха — нерідна матір для дітей чоловіка від його попереднього шлюбу.",
            });
        modelBuilder.Entity<Text>().HasData(
            new Text
            {
                Id = 1,
                Title = "Дитинство та юність",
                TextContent = @"Тарас Шевченко народився 9 березня 1814 року в селі Моринці Пединівської волості Звенигородського повіту Київської губернії. Був третьою дитиною селян-кріпаків Григорія Івановича Шевченка та Катерини Якимівни після сестри Катерини (1804 — близько 1848) та брата Микити (1811 — близько 1870).

                    За родинними переказами, Тарасові діди й прадіди з батьківського боку походили від козака Андрія, який на початку XVIII століття прийшов із Запорізької Січі. Батьки його матері, Катерини Якимівни Бойко, були переселенцями з Прикарпаття.

                    1816 року сім'я Шевченків переїхала до села Кирилівка Звенигородського повіту, звідки походив Григорій Іванович. Дитячі роки Тараса пройшли в цьому селі. 1816 року народилася Тарасова сестра Ярина, 1819 року — сестра Марія, а 1821 року народився Тарасів брат Йосип.

                    Восени 1822 року Тарас Шевченко почав учитися грамоти у дяка Совгиря. Тоді ж ознайомився з творами Григорія Сковороди.

                    10 лютого 1823 року його старша сестра Катерина вийшла заміж за Антона Красицького — селянина із Зеленої Діброви, а 1 вересня 1823 року від тяжкої праці й злиднів померла мати Катерина. 

                    19 жовтня 1823 року батько одружився вдруге з удовою Оксаною Терещенко, в якої вже було троє дітей. Вона жорстоко поводилася з нерідними дітьми, зокрема з малим Тарасом. 1824 року народилася Тарасова сестра Марія — від другого батькового шлюбу.

                    Хлопець чумакував із батьком. Бував у Звенигородці, Умані, Єлисаветграді (тепер Кропивницький). 21 березня (2 квітня) 1825 року батько помер, і невдовзі мачуха повернулася зі своїми трьома дітьми до Моринців. Зрештою Тарас змушений був залишити домівку. Деякий час Тарас жив у свого дядька Павла, який після смерті його батька став опікуном сиріт. Дядько Павло був «великий катюга»; Тарас працював у нього, разом із наймитом у господарстві, але у підсумку не витримав тяжких умов життя й пішов у найми до нового кирилівського дяка Петра Богорського.

                    Як попихач носив воду, опалював школу, обслуговував дяка, читав псалтир над померлими і вчився. Не стерпівши знущань Богорського й відчуваючи великий потяг до живопису, Тарас утік від дяка й почав шукати в навколишніх селах учителя-маляра. Кілька днів наймитував і «вчився» малярства в диякона Єфрема. Також мав учителів-малярів із села Стеблева, Канівського повіту та із села Тарасівки Звенигородського повіту. 1827 року він пас громадську отару в Кирилівці й там зустрічався з Оксаною Коваленко. Згодом подругу свого дитинства поет не раз згадає у своїх творах і присвятить їй поему «Мар'яна-черниця».",

                StreetcodeId = 1
            },
            new Text
            {
                Id = 2,
                Title = "Юність і навчання",
                TextContent = "Батьки М. І. Костомарова намагалися прищепити сину вільнолюбні ідеї і дати добру освіту." +
                    " Тому вже з 10 років М. Костомарова відправили навчатися до Московського пансіону, а згодом до Воронезької гімназії, яку той закінчив 1833 р." +
                    "\r\n\r\n1833 р. М. І. Костомаров вступає на історико-філологічний факультет Харківського університету. " +
                    "Вже у цьому навчальному закладі він проявив непересічні здібності до навчання." +
                    "\r\n\r\nВ університеті Микола Костомаров вивчав стародавні й нові мови, цікавився античною історією, німецькою філософією" +
                    " і новою французькою літературою, учився грати на фортепіано, пробував писати вірші. Зближення з гуртком українських романтиків" +
                    " Харківського університету незабаром визначило його захоплення переважно фольклором і козацьким минулим України." +
                    "\r\n\r\nУ ті роки у Харківському університеті навколо професора-славіста і літератора-романтика І. Срезневського сформувався гурток студентів," +
                    " захоплених збиранням зразків української народної пісенної творчості. Вони сприймали фольклор як вираження народного духу, самі складали вірші," +
                    " балади і ліричні пісні, звертаючись до народної творчості.\r\n\r\nКостомаров в університетські роки дуже багато читав. Перевантаження позначилося" +
                    " на його здоров'ї — ще за студентства значно погіршився зір.\r\n\r\nНа світогляд М. І. Костомарова вплинули професор грецької літератури" +
                    " Харківського університету А. О. Валицький та професор всесвітньої історії М. М. Лунін." +
                    "\r\n\r\n1836 р. М. І. Костомаров закінчив університет, а в січні 1837 р. склав іспити на ступінь кандидата й отримав направлення" +
                    " у Кінбурнський 7-й драгунський полк юнкером.\r\n\r\nУ січні 1837 року Костомаров склав іспити з усіх предметів, і 8 грудня 1837" +
                    " року його затвердили в статусі кандидата.",
                StreetcodeId = 2
            },
            new Text
            {
                Id = 3,
                Title = "Життєпис",
                TextContent = "Народився у дворянській родині на хуторі Мотронівка (нині у межах с. Оленівка поблизу Борзни)." +
                    "\r\n\r\nУ 1843–1846 роках здобув вищу освіту на історико-філологічному факультеті Київського Імператорського університету св. Володимира." +
                    "\r\n\r\n1846–1847 — учитель Петровського кадетського корпусу у Полтаві." +
                    "\r\n\r\nРазом з М. Костомаровим і М. Гулаком був організатором Кирило-Мефодіївського братства. Брав участь у створенні" +
                    " «Статуту Слов'янського братства св. Кирила і Мефодія». Автор «Записки» — пояснень до статуту братства. Розвивав ідеї християнського соціалізму, " +
                    "виступав за об'єднання всіх слов'янських народів у республіканську федерацію, в якій провідну роль відводив Україні." +
                    "\r\n1847 — 10 квітня був заарештований у Варшаві. Засланий до Олонецької губернії під нагляд поліції. Служив у Петрозаводському губернському правлінні." +
                    "\r\n\r\n1856 — звільнений із заслання. Оселився у Санкт-Петербурзі, де став активним членом місцевого гуртка українців." +
                    "\r\n\r\n1861–1862 — редактор першого українського щомісячного журналу «Основа»." +
                    "\r\n\r\nЗгодом служив у Варшаві. Підтримував зв'язки з Галичиною, співпрацював у часописах «Мета» і «Правда»." +
                    "\r\n\r\nОстанні роки життя провів на хуторі Мотронівці.",
                StreetcodeId = 3
            },
            new Text
            {
                Id = 4,
                Title = "визволення Херсона",
                TextContent = "Експерти пояснили, що дасть херсонська перемога українським силам" +
                    "\r\n\r\nНа тлі заяв окупантів про відведення військ та сил рф від Херсона та просування ЗСУ на херсонському напрямку українські бійці" +
                    " можуть отримати вогневий контроль над найважливішими дорогами Криму. Більше того, звільнення облцентру переріже постачання зброї для росії.",
                StreetcodeId = 4
            });
        modelBuilder.Entity<PersonStreetcode>().HasData(
            new PersonStreetcode
            {
                Id = 1,
                Index = 1,
                TransliterationUrl = "taras-shevchenko",
                Teaser = "Тара́с Григо́рович Шевче́нко (25 лютого (9 березня) 1814, с. Моринці, Київська губернія," +
                         " Російська імперія (нині Звенигородський район, Черкаська область, Україна) — 26 лютого (10 березня) 1861, " +
                         "Санкт-Петербург, Російська імперія) — український поет, прозаїк, мислитель, живописець, гравер, етнограф, громадський діяч. " +
                         "Національний герой і символ України. Діяч українського національного руху, член Кирило-Мефодіївського братства. " +
                         "Академік Імператорської академії мистецтв",
                ViewCount = 0,
                CreatedAt = DateTime.Now,
                DateString = "9 березня 1814 — 10 березня 1861",
                EventStartOrPersonBirthDate = new DateTime(1814, 3, 9),
                EventEndOrPersonDeathDate = new DateTime(1861, 3, 10),
                FirstName = "Тарас",
                Rank = "Григорович",
                LastName = "Шевченко",
                Title = "Тарас Шевченко",
                Alias = "Кобзар"
            },
            new PersonStreetcode
            {
                Id = 2,
                Index = 2,
                TransliterationUrl = "mykola-kostomarov",
                DateString = "9 березня 1814 — 10 березня 1861",
                Title = "Мико́ла Костома́ров",
                Teaser = "Мико́ла Іва́нович Костома́ров (4 (16) травня 1817, с. Юрасівка, Острогозький повіт, Воронезька губернія — 7 (19) квітня 1885, Петербург)" +
                    " — видатний український[8][9][10][11][12] історик, етнограф, прозаїк, поет-романтик, мислитель, громадський діяч, етнопсихолог[13][14][15]. " +
                    "\r\n\r\nБув співзасновником та активним учасником слов'янофільсько-українського київського об'єднання «Кирило - Мефодіївське братство»." +
                    " У 1847 році за участь в українофільському братстві Костомарова арештовують та перевозять з Києва до Петербурга,де він і провів решту свого життя.",
                ViewCount = 0,
                CreatedAt = DateTime.Now,
                EventStartOrPersonBirthDate = new DateTime(1817, 5, 4),
                EventEndOrPersonDeathDate = new DateTime(1885, 4, 7),
                FirstName = "Мико́ла",
                Rank = "Іва́нович",
                LastName = "Костома́ров",
            },
            new PersonStreetcode
            {
                Id = 3,
                Index = 3,
                TransliterationUrl = "vasyl-biloservky",
                Title = "Василь Білозерський",
                DateString = "2 січня 1825 — 20 лютого 1899",
                Teaser = "Білозерський Василь Михайлович (1825, хутір Мотронівка, Чернігівщина — 20 лютого (4 березня) 1899) — український громадсько-політичний" +
                    " і культурний діяч, журналіст.",
                ViewCount = 0,
                CreatedAt = DateTime.Now,
                EventStartOrPersonBirthDate = new DateTime(1825, 1, 2),
                EventEndOrPersonDeathDate = new DateTime(1899, 2, 20),
                FirstName = "Василь",
                Rank = "Михайлович",
                LastName = "Білозерський"
            },
            new PersonStreetcode
            {
                Id = 5,
                Index = 5,
                TransliterationUrl = "volodymir-varfolomiy-kropyvnitsky-shevchenkivkyski",
                Teaser = "some teaser",
                DateString = "2 січня 1825 — 20 лютого 1899",
                Title = "Володимир-Варфоломей Кропивницький-Шевченківський",
                ViewCount = 0,
                CreatedAt = DateTime.Now,
                EventStartOrPersonBirthDate = new DateTime(1825, 1, 2),
                EventEndOrPersonDeathDate = new DateTime(1899, 2, 20),
                FirstName = "Володимир-Варфоломей",
                LastName = "Кропивницький-Шевченківський"
            },
            new PersonStreetcode
            {
                Id = 6,
                Index = 6,
                TransliterationUrl = "lesya-ukrainka",
                Teaser = "some teaser",
                ViewCount = 0,
                CreatedAt = DateTime.Now,
                DateString = "2 січня 1825 — 20 лютого 1899",
                EventStartOrPersonBirthDate = new DateTime(1825, 1, 2),
                EventEndOrPersonDeathDate = new DateTime(1899, 2, 20),
                FirstName = "Леся",
                LastName = "Українка",
                Title = "Леся Українка",
                Alias = "Лариса Косач"
            },
            new PersonStreetcode
            {
                Id = 7,
                Index = 7,
                TransliterationUrl = "ivan-mazepa",
                Teaser = "some teaser",
                ViewCount = 0,
                CreatedAt = DateTime.Now,
                DateString = "2 січня 1825 — 20 лютого 1899",
                EventStartOrPersonBirthDate = new DateTime(1825, 1, 2),
                EventEndOrPersonDeathDate = new DateTime(1899, 2, 20),
                FirstName = "Іван",
                Title = "Іван Мазепа",
                LastName = "Мазепа"
            });
        modelBuilder.Entity<EventStreetcode>().HasData(
            new EventStreetcode
            {
                Id = 4,
                Index = 4,
                TransliterationUrl = "svilnennia-chersonu",
                DateString = "11 листопада 2022",
                Teaser = "Звільнення Херсона (11 листопада 2022) — відвоювання" +
                    " Збройними силами України (ЗСУ) міста Херсона та інших районів Херсонської області" +
                    " та частини Миколаївської області на правому березі Дніпра, тоді як збройні сили РФ" +
                    " Сили відійшли на лівий берег (відомий як відхід росіян" +
                    " з Херсона, 9–11 листопада 2022 р.).",
                ViewCount = 1000,
                CreatedAt = DateTime.Now,
                EventStartOrPersonBirthDate = new DateTime(2022, 11, 9),
                EventEndOrPersonDeathDate = new DateTime(2022, 11, 11),
                Title = "Звільнення Херсона"
            });

        modelBuilder.Entity<RelatedTerm>().HasData(
            new RelatedTerm
            {
                Id = 1,
                Word = "кріпаків",
                TermId = 3,
            });

        modelBuilder.Entity<TimelineItem>().HasData(
            new TimelineItem
            {
                Id = 1,
                Date = new DateTime(1831, 1, 1),
                Title = "Перші роки в Петербурзі",
                Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                            "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                            " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року."
            },
            new TimelineItem
            {
                Id = 2,
                Date = new DateTime(1830, 1, 1),
                Title = "Учень Петербурзької академії мистецтв",
                Description = "Засвідчивши свою відпускну в петербурзькій Палаті цивільного суду, Шевченко став учнем Академії мистецтв," +
                    " де його наставником став К. Брюллов. За словами Шевченка: «настала найсвітліша доба його життя, незабутні, золоті дні»" +
                    " навчання в Академії мистецтв, яким він присвятив у 1856 році автобіографічну повість «Художник»."
            }, new TimelineItem
            {
                Id = 3,
                Date = new DateTime(1832, 1, 1),
                Title = "Перші роки в Петербурзі",
                Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                            "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                            " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року."
            }, new TimelineItem
            {
                Id = 4,
                Date = new DateTime(1833, 1, 1),
                Title = "Перші роки в Петербурзі",
                Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                            "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                            " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року."
            }, new TimelineItem
            {
                Id = 5,
                Date = new DateTime(1834, 1, 1),
                Title = "Перші роки в Петербурзі",
                Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                            "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                            " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року."
            }, new TimelineItem
            {
                Id = 6,
                Date = new DateTime(1834, 1, 1),
                Title = "Перші роки в Петербурзі",
                Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                            "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                            " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року."
            }, new TimelineItem
            {
                Id = 7,
                Date = new DateTime(1834, 1, 1),
                Title = "Перші роки в Петербурзі",
                Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                            "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                            " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року."
            }, new TimelineItem
            {
                Id = 8,
                Date = new DateTime(1834, 1, 1),
                Title = "Перші роки в Петербурзі",
                Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                            "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                            " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року."
            }, new TimelineItem
            {
                Id = 9,
                Date = new DateTime(1835, 1, 1),
                Title = "Перші роки в Петербурзі",
                Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                            "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                            " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року."
            }, new TimelineItem
            {
                Id = 10,
                Date = new DateTime(1836, 1, 1),
                Title = "Перші роки в Петербурзі",
                Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                            "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                            " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року."
            });
        modelBuilder.Entity<TransactionLink>().HasData(
            new TransactionLink
            {
                Id = 1,
                Url = "https://streetcode/1",
                QrCodeUrl = "https://qrcode/1",
                StreetcodeId = 1
            },
            new TransactionLink
            {
                Id = 2,
                Url = "https://streetcode/2",
                QrCodeUrl = "https://qrcode/2",
                StreetcodeId = 2
            },
            new TransactionLink
            {
                Id = 3,
                Url = "https://streetcode/3",
                QrCodeUrl = "https://qrcode/3",
                StreetcodeId = 3
            },
            new TransactionLink
            {
                Id = 4,
                Url = "https://streetcode/4",
                QrCodeUrl = "https://qrcode/4",
                StreetcodeId = 4
            });
        modelBuilder.Entity<RelatedFigure>().HasData(
            new RelatedFigure
            {
                ObserverId = 1,
                TargetId = 2
            },
            new RelatedFigure
            {
                ObserverId = 1,
                TargetId = 3
            },
            new RelatedFigure
            {
                ObserverId = 2,
                TargetId = 3
            },
            new RelatedFigure
            {
                ObserverId = 1,
                TargetId = 4
            },
            new RelatedFigure
            {
                ObserverId = 5,
                TargetId = 1
            },
            new RelatedFigure
            {
                ObserverId = 6,
                TargetId = 1
            },
            new RelatedFigure
            {
                ObserverId = 7,
                TargetId = 1
            });
    }
}