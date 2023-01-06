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
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Persistence;

namespace Streetcode.DAL.Extensions;

public static class ModelBuilderExtensions
{
    public static void SeedData(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Subtitle>().HasData(
            new Subtitle
            {
                Id = 1,
                Status = SubtitleStatus.Editor,
                FirstName = "Dmytro",
                LastName = "Buchkovsky",
                Description = "description",
                Url = "https://t.me/MaisterD",
                StreetcodeId = 1
            },
            new Subtitle
            {
                Id = 2,
                Status = SubtitleStatus.Illustrator,
                FirstName = "Dmytro",
                LastName = "Buchkovsky",
                Description = "description",
                Url = "https://t.me/MaisterD",
                StreetcodeId = 2
            },
            new Subtitle
            {
                Id = 3,
                Status = SubtitleStatus.Editor,
                FirstName = "Dmytro",
                LastName = "Buchkovsky",
                Description = "description",
                Url = "https://t.me/MaisterD",
                StreetcodeId = 3
            },
            new Subtitle
            {
                Id = 4,
                Status = SubtitleStatus.Editor,
                FirstName = "Oleksndr",
                LastName = "Lazarenko",
                Description = "description",
                StreetcodeId = 1
            },
            new Subtitle
            {
                Id = 5,
                Status = SubtitleStatus.Editor,
                FirstName = "Oleksndr",
                LastName = "Lazarenko",
                StreetcodeId = 2
            },
            new Subtitle
            {
                Id = 6,
                Status = SubtitleStatus.Illustrator,
                FirstName = "Yaroslav",
                LastName = "Chushenko",
                StreetcodeId = 1
            },
            new Subtitle
            {
                Id = 7,
                Status = SubtitleStatus.Illustrator,
                FirstName = "Yaroslav",
                LastName = "Chushenko",
                StreetcodeId = 3
            },
            new Subtitle
            {
                Id = 8,
                Status = SubtitleStatus.Editor,
                FirstName = "Nazarii",
                LastName = "Hovdysh",
                StreetcodeId = 4
            },
            new Subtitle
            {
                Id = 9,
                Status = SubtitleStatus.Illustrator,
                FirstName = "Tatiana",
                LastName = "Shumylo",
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
                ImageId = 2,
                Description = "«Погруддя жінки» — портрет роботи Тараса Шевченка (копія з невідомого оригіналу) виконаний ним у Вільно в 1830 році на папері італійським олівцем. Розмір 47,5 × 38. Зустрічається також під назвою «Жіноча голівка»"
            },
            new Art
            {
                Id = 2,
                ImageId = 3,
            },
            new Art
            {
                Id = 3,
                ImageId = 4
            });
        modelBuilder.Entity<Image>().HasData(
            new Image
            {
                Id = 1,
                Title = "Тарас Шевченко",
                Alt = "Портрет Тараса Шевченка",
                Url = "https://www.bing.com/images/search?view=detailV2&ccid=07Ymnt6l&id=A8ACFBEB6A3695B1E7DD2887A46505D759921BDC&thid=OIP.07Ymnt6ljB86Jx-Fy2wGUwHaKY&mediaurl=https%3A%2F%2Fproprikol.ru%2Fwp-content%2Fuploads%2F2021%2F05%2Fkartinki-taras-shevchenko-39.jpg&cdnurl=https%3A%2F%2Fth.bing.com%2Fth%2Fid%2FR.d3b6269edea58c1f3a271f85cb6c0653%3Frik%3D3BuSWdcFZaSHKA%26pid%3DImgRaw%26r%3D0&exph=1581&expw=1128&q=%d1%82%d0%b0%d1%80%d0%b0%d1%81+%d1%88%d0%b5%d0%b2%d1%87%d0%b5%d0%bd%d0%ba%d0%be&simid=608002717784548528&form=IRPRST&ck=0EF4CC2DA9612AC10DAE96953F62051F&selectedindex=1&ajaxhist=0&ajaxserp=0&vt=0&sim=11"
            },
            new Image
            {
                Id = 2,
                Title = "Погруддя жінки",
                Alt = "Тарас Шевченко: Погруддя жінки",
                Url = "https://upload.wikimedia.org/wikipedia/commons/1/10/Taras_Shevchenko_painting_0001.jpg"
            },
            new Image
            {
                Id = 3,
                Title = "Портрет Павла Васильовича Енгельгардта",
                Alt = "Тарас Шевченко: Портрет Павла Васильовича Енгельгардта",
                Url = "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Enhelhard_by_Shevchenko.jpg"
            },
            new Image
            {
                Id = 4,
                Title = "Портрет невідомого",
                Alt = "Тарас Шевченко: Портрет невідомого",
                Url = "https://uk.wikipedia.org/wiki/%D0%A1%D0%BF%D0%B8%D1%81%D0%BE%D0%BA_%D0%BA%D0%B0%D1%80%D1%82%D0%B8%D0%BD_%D1%96_%D0%BC%D0%B0%D0%BB%D1%8E%D0%BD%D0%BA%D1%96%D0%B2_%D0%A2%D0%B0%D1%80%D0%B0%D1%81%D0%B0_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%B0#/media/%D0%A4%D0%B0%D0%B9%D0%BB:Portret_nevidomoho_Shevchenko_.jpg"
            },
            new Image
            {
                Id = 5,
                Title = "Кобзар",
                Alt = "Кобзар",
                Url = "https://www.bing.com/images/search?view=detailV2&ccid=6juPycgD&id=00A2C7B1F325A9870421D651A956BCE2C851654E&thid=OIP.6juPycgDNwJ3v2Zr-kde1gHaK_&mediaurl=https%3A%2F%2Fwww.megakniga.com.ua%2Fuploads%2Fcache%2FProducts%2FProduct_images_343456%2Fd067b1_w1600.jpg&cdnurl=https%3A%2F%2Fth.bing.com%2Fth%2Fid%2FR.ea3b8fc9c803370277bf666bfa475ed6%3Frik%3DTmVRyOK8VqlR1g%26pid%3DImgRaw%26r%3D0&exph=1200&expw=809&q=%d0%ba%d0%be%d0%b1%d0%b7%d0%b0%d1%80&simid=608047540067197142&form=IRPRST&ck=4280C365AEBC65D796FBF885B3252710&selectedindex=1&ajaxhist=0&ajaxserp=0&vt=0&sim=11"
            },
            new Image
            {
                Id = 6,
                Title = "Мико́ла Костома́ров",
                Alt = "Мико́ла Костома́ров",
                Url = "https://www.bing.com/images/search?view=detailV2&ccid=KUJZwRaU&id=A53DDEBFF57BE2396FB7FA50737F83704B1BE30F&thid=OIP.KUJZwRaUjipKMLR8H91BrAAAAA&mediaurl=https%3a%2f%2fgdb.rferl.org%2f224F2B76-EE74-4B85-A78A-BF8A354FA0B1_w250_r0_s.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.294259c116948e2a4a30b47c1fdd41ac%3frik%3dD%252bMbS3CDf3NQ%252bg%26pid%3dImgRaw%26r%3d0&exph=340&expw=250&q=%d0%9c%d0%b8%d0%ba%d0%be%cc%81%d0%bb%d0%b0+%d0%9a%d0%be%d1%81%d1%82%d0%be%d0%bc%d0%b0%cc%81%d1%80%d0%be%d0%b2&simid=608030609289524022&FORM=IRPRST&ck=E08972A7A7E2CEE9B67158DDC372F92F&selectedIndex=3&ajaxhist=0&ajaxserp=0"
            },
            new Image
            {
                Id = 7,
                Title = "Василь Білозерський",
                Alt = "Василь Білозерський",
                Url = "https://www.bing.com/images/search?view=detailV2&ccid=hIQUFjAM&id=B14676F51B4A0EB314ED15283540D088B3030E28&thid=OIP.hIQUFjAMGwOt7f7ujR44aQAAAA&mediaurl=https%3a%2f%2fnaurok-test.nyc3.cdn.digitaloceanspaces.com%2fuploads%2ftest%2f229691%2f36505%2f276576_1582512990.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.84841416300c1b03adedfeee8d1e3869%3frik%3dKA4Ds4jQQDUoFQ%26pid%3dImgRaw%26r%3d0&exph=351&expw=240&q=%d0%92%d0%b0%d1%81%d0%b8%d0%bb%d1%8c+%d0%91%d1%96%d0%bb%d0%be%d0%b7%d0%b5%d1%80%d1%81%d1%8c%d0%ba%d0%b8%d0%b9&simid=608001205960330039&FORM=IRPRST&ck=07DE282212732F4C0712D614C87002F3&selectedIndex=1&ajaxhist=0&ajaxserp=0"
            },
            new Image
            {
                Id = 8,
                Title = "Звільнення Херсона",
                Alt = "Звільнення Херсона",
                Url = "https://www.bing.com/images/search?view=detailV2&ccid=F5o3vrW9&id=5409686EF1396243251CE5AF505766A0A2D0662E&thid=OIP.F5o3vrW9jZJ9ECMgkmevTwHaFj&mediaurl=https%3a%2f%2fstorage1.censor.net%2fimages%2f1%2f7%2f9%2fa%2f179a37beb5bd8d927d1023209267af4f%2foriginal.jpg&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.179a37beb5bd8d927d1023209267af4f%3frik%3dLmbQoqBmV1Cv5Q%26pid%3dImgRaw%26r%3d0&exph=720&expw=960&q=%d0%b2%d0%b8%d0%b7%d0%b2%d0%be%d0%bb%d0%b5%d0%bd%d0%bd%d1%8f+%d1%85%d0%b5%d1%80%d1%81%d0%be%d0%bd%d1%83&simid=608050323200235844&FORM=IRPRST&ck=C9A86B9D5EBBADF456F315DFD0BA990B&selectedIndex=3&ajaxhist=0&ajaxserp=0"
            },
            new Image
            {
                Id = 9,
                Title = "book",
                Alt = "book",
                Url = "https://marvistamom.com/wp-content/uploads/books3.jpg"
            },
            new Image
            {
                Id = 10,
                Title = "video",
                Alt = "video",
                Url = "https://www.earnmydegree.com/sites/all/files/public/video-prod-image.jpg"
            },
            new Image
            {
                Id = 11,
                Title = "article",
                Alt = "article",
                Url = "https://images.laws.com/constitution/constitutional-convention.jpg"
            });
        modelBuilder.Entity<Audio>().HasData(
            new Audio
            {
                Id = 1,
                Title = "audio1",
                Description = "for streetcode1",
                Url = "https://somelink1",
                StreetcodeId = 1
            },
            new Audio
            {
                Id = 2,
                Title = "audio2",
                Description = "for streetcode2",
                Url = "https://somelink2",
                StreetcodeId = 2
            },
            new Audio
            {
                Id = 3,
                Title = "audio3",
                Description = "for streetcode3",
                Url = "https://somelink3",
                StreetcodeId = 3
            },
            new Audio
            {
                Id = 4,
                Title = "audio4",
                Description = "for streetcode4",
                Url = "https://somelink4",
                StreetcodeId = 4
            });
        modelBuilder.Entity<ToponymCoordinate>().HasData(
            new ToponymCoordinate
            {
                Id = 1,
                Latitude = 49.8429M,
                Longtitude = 24.0311M,
                ToponymId = 1
            },
            new ToponymCoordinate
            {
                Id = 2,
                Latitude = 50.4500M,
                Longtitude = 30.5233M,
                ToponymId = 1
            },
            new ToponymCoordinate
            {
                Id = 3,
                Latitude = 47.5M,
                Longtitude = 37.32M,
                ToponymId = 1
            },
            new ToponymCoordinate
            {
                Id = 4,
                Latitude = 50.4600M,
                Longtitude = 30.5243M,
                ToponymId = 2
            },
            new ToponymCoordinate
            {
                Id = 5,
                Latitude = 50.4550M,
                Longtitude = 30.5238M,
                ToponymId = 2
            },
            new ToponymCoordinate
            {
                Id = 8,
                Latitude = 46.3950M,
                Longtitude = 32.3738M,
                ToponymId = 3
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
                Title = "SoftServe",
                Description = "Developers",
                LogoUrl = "https://www.bing.com/images/search?view=detailV2&ccid=g3DnkGqg&id=98C6F1FDD6CDA685A3DE2AD392FAC228180A28CC&thid=OIP.g3DnkGqgmhKFWM2ct5mXrAHaHa&mediaurl=https%3a%2f%2fyt3.ggpht.com%2fa-%2fAN66SAxaiWXvFxW9BUQ32pzQ5tv5UuXz2fLZ20LaMg%3ds900-mo-c-c0xffffffff-rj-k-no&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.8370e7906aa09a128558cd9cb79997ac%3frik%3dzCgKGCjC%252bpLTKg%26pid%3dImgRaw%26r%3d0&exph=900&expw=900&q=softserve&simid=608013145967840441&FORM=IRPRST&ck=C08BED6E397D35D8A4824BB4B78EBCE8&selectedIndex=1&ajaxhist=0&ajaxserp=0",
                TargetUrl = "https://www.softserveinc.com/en-us"
            },
            new Partner
            {
                Id = 2,
                Title = "parimatch",
                LogoUrl = "https://www.bing.com/images/search?view=detailV2&ccid=9TObzn%2ba&id=21D8755FE7846CE9660BC2365F5EE70417D31DA7&thid=OIP.9TObzn-a15MsLhdfHh1e_gHaE8&mediaurl=https%3a%2f%2fi2.wp.com%2feuropeangaming.eu%2fportal%2fwp-content%2fuploads%2f2020%2f02%2f5-10.jpg%3ffit%3d1200%252C800%26ssl%3d1&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.f5339bce7f9ad7932c2e175f1e1d5efe%3frik%3dpx3TFwTnXl82wg%26pid%3dImgRaw%26r%3d0&exph=800&expw=1200&q=parimatch&simid=607987165708116105&FORM=IRPRST&ck=BA1164F39CC2BBD1CE20F50A93602E5C&selectedIndex=1&ajaxhist=0&ajaxserp=0",
                TargetUrl = "https://parimatch.com/"
            },
            new Partner
            {
                Id = 3,
                Title = "comunity partner",
                LogoUrl = "https://www.bing.com/images/search?view=detailV2&ccid=9TObzn%2ba&id=21D8755FE7846CE9660BC2365F5EE70417D31DA7&thid=OIP.9TObzn-a15MsLhdfHh1e_gHaE8&mediaurl=https%3a%2f%2fi2.wp.com%2feuropeangaming.eu%2fportal%2fwp-content%2fuploads%2f2020%2f02%2f5-10.jpg%3ffit%3d1200%252C800%26ssl%3d1&cdnurl=https%3a%2f%2fth.bing.com%2fth%2fid%2fR.f5339bce7f9ad7932c2e175f1e1d5efe%3frik%3dpx3TFwTnXl82wg%26pid%3dImgRaw%26r%3d0&exph=800&expw=1200&q=parimatch&simid=607987165708116105&FORM=IRPRST&ck=BA1164F39CC2BBD1CE20F50A93602E5C&selectedIndex=1&ajaxhist=0&ajaxserp=0",
                TargetUrl = "https://parimatch.com/"
            });
        modelBuilder.Entity<PartnerSourceLink>().HasData(
            new PartnerSourceLink
            {
                Id = 1,
                Title = "LinkedIn",
                LogoUrl = string.Empty,
                TargetUrl = "https://www.linkedin.com/company/softserve/",
                PartnerId = 1
            },
            new PartnerSourceLink
            {
                Id = 2,
                Title = "Instagram",
                LogoUrl = string.Empty,
                TargetUrl = "https://www.instagram.com/softserve_people/",
                PartnerId = 1
            },
            new PartnerSourceLink
            {
                Id = 3,
                Title = "facebook",
                LogoUrl = string.Empty,
                TargetUrl = "https://www.facebook.com/SoftServeCompany",
                PartnerId = 1
            });
        modelBuilder.Entity<StreetcodePartner>().HasData(
            new StreetcodePartner
            {
                IsSponsor = true,
                PartnerId = 1,
                StreetcodeId = 1
            },
            new StreetcodePartner
            {
                IsSponsor = true,
                PartnerId = 1,
                StreetcodeId = 2
            },
            new StreetcodePartner
            {
                IsSponsor = false,
                PartnerId = 1,
                StreetcodeId = 3
            },
            new StreetcodePartner
            {
                IsSponsor = true,
                PartnerId = 2,
                StreetcodeId = 1
            },
            new StreetcodePartner
            {
                IsSponsor = false,
                PartnerId = 2,
                StreetcodeId = 2
            },
            new StreetcodePartner
            {
                IsSponsor = false,
                PartnerId = 3,
                StreetcodeId = 3
            },
            new StreetcodePartner
            {
                IsSponsor = true,
                PartnerId = 1,
                StreetcodeId = 4
            },
            new StreetcodePartner
            {
                IsSponsor = true,
                PartnerId = 2,
                StreetcodeId = 4
            });
        modelBuilder.Entity<SourceLink>().HasData(
            new SourceLink
            {
                Id = 1,
                Title = "Вікіпедія",
                Url = "https://uk.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE_%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%93%D1%80%D0%B8%D0%B3%D0%BE%D1%80%D0%BE%D0%B2%D0%B8%D1%87",
                StreetcodeId = 1
            },
            new SourceLink
            {
                Id = 2,
                Title = "Кобзар",
                Url = "https://uk.wikipedia.org/wiki/%D0%A4%D0%B0%D0%B9%D0%BB:%D0%A2%D0%B0%D1%80%D0%B0%D1%81_%D0%A8%D0%B5%D0%B2%D1%87%D0%B5%D0%BD%D0%BA%D0%BE._%D0%9A%D0%BE%D0%B1%D0%B7%D0%B0%D1%80._1840.pdf",
                StreetcodeId = 1
            },
            new SourceLink
            {
                Id = 3,
                Title = "Св'яткування звільнення",
                Url = "https://tsn.ua/ukrayina/z-pisnyami-i-tostami-zvilnennya-hersona-svyatkuyut-v-inshih-mistah-ukrayini-i-navit-za-kordonom-video-2200096.html",
                StreetcodeId = 4
            });
        modelBuilder.Entity<SourceLinkCategory>().HasData(
            new SourceLinkCategory
            {
                Id = 1,
                Title = "book",
                ImageId = 9
            },
            new SourceLinkCategory
            {
                Id = 2,
                Title = "video",
                ImageId = 10
            },
            new SourceLinkCategory
            {
                Id = 3,
                Title = "article",
                ImageId = 11
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
                    " а 7 травня Шевченкові видали відпускну."
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
            });
        modelBuilder.Entity<Text>().HasData(
            new Text
            {
                Id = 1,
                Title = "Дитинство та юність",
                TextContent = "Тарас Шевченко народився 9 березня 1814 року в селі Моринці" +
                    " Пединівської волості Звенигородського повіту Київської губернії. Був третьою дитиною селян-кріпаків Григорія Івановича Шевченка" +
                    " та Катерини Якимівни після сестри Катерини (1804 — близько 1848) та брата Микити (1811 — близько 1870)." +
                    "\r\n\r\nЗа родинними переказами, Тарасові діди й прадіди з батьківського боку походили від козака Андрія, який на початку XVIII століття" +
                    " прийшов із Запорізької Січі. Батьки його матері, Катерини Якимівни Бойко, були переселенцями з Прикарпаття.\r\n\r\n1816 року" +
                    " сім'я Шевченків переїхала до села Кирилівка Звенигородського повіту, звідки походив Григорій Іванович." +
                    " Дитячі роки Тараса пройшли в цьому селі. 1816 року народилася Тарасова сестра Ярина, 1819 року — сестра Марія," +
                    " а 1821 року народився Тарасів брат Йосип.\r\n\r\nВосени 1822 року Тарас Шевченко почав учитися грамоти у дяка Совгиря." +
                    " Тоді ж ознайомився з творами Григорія Сковороди.\r\n\r\n10 лютого 1823 року його старша сестра Катерина вийшла заміж за " +
                    "Антона Красицького — селянина із Зеленої Діброви, а 1 вересня 1823 року від тяжкої праці й злиднів померла мати Катерина. " +
                    "\r\n\r\n19 жовтня 1823 року батько одружився вдруге з удовою Оксаною Терещенко, в якої вже було троє дітей." +
                    " Вона жорстоко поводилася з нерідними дітьми, зокрема з малим Тарасом. 1824 року народилася Тарасова сестра Марія — від другого" +
                    " батькового шлюбу.",
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
                Teaser = "Тара́с Григо́рович Шевче́нко (25 лютого (9 березня) 1814, с. Моринці, Київська губернія," +
                         " Російська імперія (нині Звенигородський район, Черкаська область, Україна) — 26 лютого (10 березня) 1861, " +
                         "Санкт-Петербург, Російська імперія) — український поет, прозаїк, мислитель, живописець, гравер, етнограф, громадський діяч. " +
                         "Національний герой і символ України. Діяч українського національного руху, член Кирило-Мефодіївського братства. " +
                         "Академік Імператорської академії мистецтв",
                ViewCount = 0,
                CreatedAt = DateTime.Now,
                EventStartOrPersonBirthDate = new DateTime(1814, 3, 9),
                EventEndOrPersonDeathDate = new DateTime(1861, 3, 10),
                FirstName = "Тарас",
                Rank = "Григорович",
                LastName = "Шевченко"
            },
            new PersonStreetcode
            {
                Id = 2,
                Index = 2,
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
                LastName = "Костома́ров"
            },
            new PersonStreetcode
            {
                Id = 3,
                Index = 3,
                Teaser = "Білозерський Василь Михайлович (1825, хутір Мотронівка, Чернігівщина — 20 лютого (4 березня) 1899) — український громадсько-політичний" +
                    " і культурний діяч, журналіст.",
                ViewCount = 0,
                CreatedAt = DateTime.Now,
                EventStartOrPersonBirthDate = new DateTime(1825, 1, 2),
                EventEndOrPersonDeathDate = new DateTime(1899, 2, 20),
                FirstName = "Василь",
                Rank = "Михайлович",
                LastName = "Білозерський"
            });
        modelBuilder.Entity<EventStreetcode>().HasData(
            new EventStreetcode
            {
                Id = 4,
                Index = 4,
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
            });
        modelBuilder.Entity<Toponym>().HasData(
            new Toponym
            {
                Id = 1,
                Title = "вулиця Шевченка"
            },
            new Toponym
            {
                Id = 2,
                Title = "парк Шевченка"
            },
            new Toponym
            {
                Id = 3,
                Title = "місто Херсон"
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
            });
        modelBuilder.Entity<SourceLinkSubCategory>().HasData(
            new SourceLinkSubCategory
            {
                Id = 1,
                Title = "Книги про Грушевьского",
                SourceLinkCategoryId = 3,
            },
            new SourceLinkSubCategory
            {
                Id = 2,
                Title = "Праці Грушевьского",
                SourceLinkCategoryId = 1,
            },
            new SourceLinkSubCategory
            {
                Id = 3,
                Title = "Фільми про Грушевьского",
                SourceLinkCategoryId = 1,
            });
    }
}