using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Streetcode.BLL.Services.BlobStorageService;
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
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Entities.Transactions;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Persistence;
using Streetcode.DAL.Repositories.Realizations.Base;

namespace Streetcode.WebApi.Extensions
{
    public static class SeedingLocalExtension
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                Directory.CreateDirectory(app.Configuration.GetValue<string>("Blob:BlobStorePath"));
                var dbContext = scope.ServiceProvider.GetRequiredService<StreetcodeDbContext>();
                var blobOptions = app.Services.GetRequiredService<IOptions<BlobEnvironmentVariables>>();
                string blobPath = app.Configuration.GetValue<string>("Blob:BlobStorePath");
                var repo = new RepositoryWrapper(dbContext);
                var blobService = new BlobService(blobOptions, repo);
                string initialDataImagePath = "../Streetcode.DAL/InitialData/images.json";
                string initialDataAudioPath = "../Streetcode.DAL/InitialData/audios.json";
                if (!dbContext.Images.Any())
                {
                    string imageJson = File.ReadAllText(initialDataImagePath, Encoding.UTF8);
                    string audiosJson = File.ReadAllText(initialDataAudioPath, Encoding.UTF8);
                    var imgfromJson = JsonConvert.DeserializeObject<List<Image>>(imageJson);
                    var audiosfromJson = JsonConvert.DeserializeObject<List<Audio>>(audiosJson);

                    foreach (var img in imgfromJson)
                    {
                        string filePath = Path.Combine(blobPath, img.BlobName);
                        if (!File.Exists(filePath))
                        {
                            blobService.SaveFileInStorageBase64(img.Base64, img.BlobName.Split('.')[0], img.BlobName.Split('.')[1]);
                        }
                    }

                    foreach (var audio in audiosfromJson)
                    {
                        string filePath = Path.Combine(blobPath, audio.BlobName);
                        if (!File.Exists(filePath))
                        {
                            blobService.SaveFileInStorageBase64(audio.Base64, audio.BlobName.Split('.')[0], audio.BlobName.Split('.')[1]);
                        }
                    }

                    dbContext.Images.AddRange(imgfromJson);

                    await dbContext.SaveChangesAsync();

                    if (!dbContext.Responses.Any())
                    {
                        dbContext.Responses.AddRange(
                            new Response
                            {
                                Name = "Alex",
                                Description = "Good Job",
                                Email = "dmytrobuchkovsky@gmail.com"
                            },
                            new Response
                            {
                                Name = "Danyil",
                                Description = "Nice project",
                                Email = "dt210204@gmail.com"
                            });

                        await dbContext.SaveChangesAsync();
                    }

                    if (!dbContext.Terms.Any())
                    {
                        dbContext.Terms.AddRange(
                            new Term
                            {
                                Title = "етнограф",
                                Description = "Етнографія — суспільствознавча наука, об'єктом дослідження якої є народи, їхня культура і побут, походження, розселення," +
                                " процеси культурно-побутових відносин на всіх етапах історії людства."
                            },
                            new Term
                            {
                                Title = "гравер",
                                Description = "Гра́фіка — вид образотворчого мистецтва, для якого характерна перевага ліній і штрихів, використання контрастів білого та" +
                                    " чорного та менше, ніж у живописі, використання кольору. Твори можуть мати як монохромну, так і поліхромну гаму."
                            },
                            new Term
                            {
                                Title = "кріпак",
                                Description = "Кріпа́цтво, або кріпосне́ право, у вузькому сенсі — правова система, або система правових норм при феодалізмі, яка встановлювала" +
                                    " залежність селянина від феодала й неповну власність феодала на селянина."
                            },
                            new Term
                            {
                                Title = "мачуха",
                                Description = "Ма́чуха — нерідна матір для дітей чоловіка від його попереднього шлюбу.",
                            });

                        await dbContext.SaveChangesAsync();

                        if (!dbContext.RelatedTerms.Any())
                        {
                            dbContext.RelatedTerms.AddRange(
                                new RelatedTerm
                                {
                                    Word = "кріпаків",
                                    TermId = 3,
                                });

                            await dbContext.SaveChangesAsync();
                        }
                    }

                    if (!dbContext.TeamMembers.Any())
                    {
                        dbContext.AddRange(
                            new TeamMember
                            {
                                FirstName = "Inna",
                                LastName = "Krupnyk",
                                ImageId = 25,
                                Description = "У 1894 році Грушевський за рекомендацією Володимира Антоновича призначений\r\nна посаду ординарного професора",
                                IsMain = true
                            },
                            new TeamMember
                            {
                                FirstName = "Danyil",
                                LastName = "Terentiev",
                                ImageId = 26,
                                Description = "У 1894 році Грушевський за рекомендацією Володимира Антоновича призначений\r\nна посаду ординарного професора",
                                IsMain = true
                            },
                            new TeamMember
                            {
                                FirstName = "Nadia",
                                LastName = "Kischchuk",
                                ImageId = 27,
                                Description = "У 1894 році Грушевський за рекомендацією Володимира Антоновича призначений\r\nна посаду ординарного професора",
                                IsMain = true
                            });

                        await dbContext.SaveChangesAsync();

                        if (!dbContext.Positions.Any())
                        {
                            dbContext.Positions.AddRange(
                                new Positions
                                {
                                    Position = "Голова і засновниця ГО"
                                });

                            await dbContext.SaveChangesAsync();

                            if (!dbContext.TeamMemberPosition.Any())
                            {
                                dbContext.TeamMemberPosition.AddRange(
                                    new TeamMemberPositions
                                    {
                                        PositionsId = 1,
                                        TeamMemberId = 1
                                    },
                                    new TeamMemberPositions
                                    {
                                        PositionsId = 1,
                                        TeamMemberId = 2
                                    },
                                    new TeamMemberPositions
                                    {
                                        PositionsId = 1,
                                        TeamMemberId = 3
                                    });

                                await dbContext.SaveChangesAsync();
                            }

                            if (!dbContext.TeamMemberLinks.Any())
                            {
                                dbContext.AddRange(
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.YouTube,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 1,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.Facebook,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 1,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.Instagram,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 1,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.Twitter,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 1,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.YouTube,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 2,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.Facebook,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 2,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.Instagram,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 2,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.Twitter,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 2,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.YouTube,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 3,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.Facebook,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 3,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.Instagram,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 3,
                                    },
                                    new TeamMemberLink
                                    {
                                        LogoType = LogoType.Twitter,
                                        TargetUrl = "https://www.youtube.com/watch?v=8kCnOqvmEp0&ab_channel=JL%7C%D0%AE%D0%9B%D0%86%D0%AF%D0%9B%D0%A3%D0%A9%D0%98%D0%9D%D0%A1%D0%AC%D0%9A%D0%90",
                                        TeamMemberId = 3,
                                    });
                                await dbContext.SaveChangesAsync();
                            }
                        }
                    }

                    if (!dbContext.Users.Any())
                    {
                        dbContext.Users.AddRange(
                            new DAL.Entities.Users.User
                            {
                                Email = "admin",
                                Role = UserRole.MainAdministrator,
                                Login = "admin",
                                Name = "admin",
                                Password = "admin",
                                Surname = "admin",
                            });

                        await dbContext.SaveChangesAsync();
                    }

                    if (!dbContext.News.Any())
                    {
                        dbContext.News.AddRange(
                            new DAL.Entities.News.News
                            {
                                Title = "27 квітня встановлюємо перший стріткод!",
                                Text = "<p>Встановлення таблички про Михайла Грушевського в м. Київ стало важливою подією для киян та гостей столиці. Вона не лише прикрашає вулицю міста, а й нагадує про значний внесок цієї визначної особистості в історію України. Це також сприяє розповсюдженню знань про Михайла Грушевського серед широкого загалу, виховує національну свідомість та гордість за власну культуру.\r\n\r\nВстановлення таблички про Михайла Грушевського в Києві є важливим кроком на шляху вшанування відомих особистостей, які внесли вагомий внесок у розвиток України. Це також показує, що в Україні дбають про збереження національної спадщини та визнання внеску видатних історичних постатей в формування національної ідентичності.\r\n\r\nУрочисте встановлення таблички про Михайла Грушевського відбулося за участі високопосадовців міста, представників наукової спільноти та громадськості. Під час церемонії відбулися промови, в яких відзначили важливість дослідницької та літературної діяльності М. Грушевського, його внесок у вивчення історії України та роль у національному відродженні.\r\n\r\nМихайло Грушевський жив і працював в Києві на початку ХХ століття. Він був визнаний одним з провідних істориків свого часу, який досліджував історію України з наукової та національної позицій. Його праці були визнані авторитетними не лише в Україні, але й у світі, і мають велике значення для розуміння минулого та формування майбутнього українського народу.\r\n\r\nТабличка з відтвореним зображенням Михайла Грушевського стала вагомим символом вшанування цієї видатної постаті. Вона стала візитівкою Києва та пам'яткою культурної спадщини України, яка привертає увагу мешканців та гостей міста. Це важливий крок на шляху до збереження національної історії, культури та національної свідомості в Україні.\r\n\r\nВстановлення таблички про Михайла Грушевського в Києві свідчить про важливість визнання історичної спадщини та внеску видатних постатей в національну свідомість. Це також є визнанням ролі М. Грушевського у формуванні української національної ідентичності та його внеску в розвиток наукової та культурної спадщини України.\r\n\r\nТабличка була встановлена на видному місці в центрі Києва, недалеко від місця, де розташовується будинок, в якому колись проживав Михайло Грушевський. Зображення на табличці передає фотографію видатного історика, а також містить кратку інформацію про його життя та діяльність.\r\n\r\nМешканці та гості Києва високо оцінюють встановлення таблички про Михайла Грушевського, яке стало ще одним кроком на шляху до вшанування історичної спадщини України. Це також важливий крок у визнанні ролі українських науковців та культурних діячів у світовому контексті.\r\n\r\nВстановлення таблички про Михайла Грушевського в Києві є однією з ініціатив, спрямованих на підтримку і розширення національної пам'яті та відтворення історичної правди. Це важливий крок на шляху до відродження національної свідомості та підкреслення значення української культурної спадщини в світовому контексті.</p>",
                                URL = "first-streetcode",
                                ImageId = 24,
                                CreationDate = DateTime.Now,
                            },
                            new DAL.Entities.News.News
                            {
                                Title = "Новий учасник команди!",
                                Text = "<p>Привітаймо нового учасника команди - Терентьєва Даниїла!. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque arcu orci, dictum at posuere a, tincidunt sit amet nibh. Donec pellentesque ac mauris tristique egestas. Vestibulum hendrerit eget nisi non viverra. Nullam ultricies sapien ac ipsum ullamcorper tristique. Mauris auctor, sapien vitae molestie ornare, libero orci fringilla velit, sed pharetra nibh augue id tellus. Mauris pulvinar vel felis convallis molestie. Integer mauris felis, ultrices nec vestibulum at, ullamcorper eu massa. Proin posuere consectetur facilisis. Nunc volutpat dictum massa, ac volutpat nisl malesuada nec.\r\n\r\nNulla nec felis quis metus efficitur efficitur ac nec est. Nulla eros quam, tincidunt at elit nec, iaculis eleifend sem. Pellentesque id sem id erat mollis fermentum non at ipsum. Donec justo ante, commodo a pharetra a, consectetur at urna. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Praesent porta, odio sed venenatis posuere, felis nibh finibus dui, placerat molestie dui libero at nisi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Duis quis nisi in nisi pulvinar ultrices.\r\n\r\nPellentesque ante nunc, mattis vitae iaculis id, sollicitudin nec tortor. Pellentesque eu lectus suscipit, sodales nunc eu, lobortis enim. Praesent tempus dolor et felis vulputate hendrerit. Nunc ut lacus.</p>",
                                URL = "danya",
                                ImageId = 28,
                                CreationDate = DateTime.Now,
                            },
                            new DAL.Entities.News.News
                            {
                                Title = "Новий учасник команди!",
                                Text = "<p>Привітаймо нового учасника команди - Скам Мастера!. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque arcu orci, dictum at posuere a, tincidunt sit amet nibh. Donec pellentesque ac mauris tristique egestas. Vestibulum hendrerit eget nisi non viverra. Nullam ultricies sapien ac ipsum ullamcorper tristique. Mauris auctor, sapien vitae molestie ornare, libero orci fringilla velit, sed pharetra nibh augue id tellus. Mauris pulvinar vel felis convallis molestie. Integer mauris felis, ultrices nec vestibulum at, ullamcorper eu massa. Proin posuere consectetur facilisis. Nunc volutpat dictum massa, ac volutpat nisl malesuada nec.\r\n\r\nNulla nec felis quis metus efficitur efficitur ac nec est. Nulla eros quam, tincidunt at elit nec, iaculis eleifend sem. Pellentesque id sem id erat mollis fermentum non at ipsum. Donec justo ante, commodo a pharetra a, consectetur at urna. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Praesent porta, odio sed venenatis posuere, felis nibh finibus dui, placerat molestie dui libero at nisi. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Duis quis nisi in nisi pulvinar ultrices.\r\n\r\nPellentesque ante nunc, mattis vitae iaculis id, sollicitudin nec tortor. Pellentesque eu lectus suscipit, sodales nunc eu, lobortis enim. Praesent tempus dolor et felis vulputate hendrerit. Nunc ut lacus.</p>",
                                URL = "scum",
                                ImageId = 29,
                                CreationDate = DateTime.Now,
                            });

                        await dbContext.SaveChangesAsync();
                    }

                    if (!dbContext.Audios.Any())
                    {
                        dbContext.Audios.AddRange(audiosfromJson);

                        await dbContext.SaveChangesAsync();

                        if (!dbContext.Streetcodes.Any())
                        {
                            dbContext.Streetcodes.AddRange(
                                new PersonStreetcode
                                {
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
                                    Alias = "Кобзар",
                                    AudioId = 1,
                                    Status = StreetcodeStatus.Published
                                },
                                new PersonStreetcode
                                {
                                    Index = 2,
                                    TransliterationUrl = "roman-ratushnyi",
                                    Teaser = "Роман був з тих, кому не байдуже. Небайдуже до свого Протасового Яру та своєї України. Талановитий, щедрий, запальний. З нового покоління українців, народжених за незалежності, мета яких — краща Україна. Інтелектуал, активіст, громадський діяч. Бунтар проти несправедливості: корупції, свавілля. Невтомний як у боротьбі з незаконною забудовою, так і в захисті рідної країни від ворога. Учасник Помаранчевої революції 2004 року та Революції гідності 2013–2014-го. Воїн, який заради України пожертвував власним життям.",
                                    ViewCount = 1,
                                    CreatedAt = DateTime.Now,
                                    DateString = "5 липня 1997 – 9 червня 2022",
                                    EventStartOrPersonBirthDate = new DateTime(1997, 7, 5),
                                    EventEndOrPersonDeathDate = new DateTime(2022, 6, 9),
                                    FirstName = "Роман",
                                    LastName = "Ратушний",
                                    Title = "Роман Ратушний (Сенека)",
                                    Alias = "Сенека",
                                    AudioId = 2,
                                    Status = StreetcodeStatus.Published
                                });

                            await dbContext.SaveChangesAsync();

                            if (!dbContext.Subtitles.Any())
                            {
                                dbContext.Subtitles.AddRange(
                                    new Subtitle
                                    {
                                        SubtitleText = "Developers: StreedCodeTeam, made with love and passion, some more text, and more text. There was Danya",
                                        StreetcodeId = 1
                                    },
                                    new Subtitle
                                    {
                                        SubtitleText = "Developers: StreedCodeTeam, made with love and passion, some more text, and more text. There was Danya",
                                        StreetcodeId = 2
                                    });

                                await dbContext.SaveChangesAsync();
                            }

                            if (!dbContext.StreetcodeCoordinates.Any())
                            {
                                dbContext.StreetcodeCoordinates.AddRange(
                                    new StreetcodeCoordinate
                                    {
                                        Latitude = 49.8429M,
                                        Longtitude = 24.0311M,
                                        StreetcodeId = 1
                                    },
                                    new StreetcodeCoordinate
                                    {
                                        Latitude = 50.4550M,
                                        Longtitude = 30.5238M,
                                        StreetcodeId = 2
                                    });

                                await dbContext.SaveChangesAsync();
                            }

                            if (!dbContext.Videos.Any())
                            {
                                dbContext.Videos.AddRange(
                                    new Video
                                    {
                                        Title = "audio1",
                                        Description = "for streetcode1",
                                        Url = "https://www.youtube.com/watch?v=VVFEi6lTpZk&ab_channel=%D0%9E%D1%81%D1%82%D0%B0%D0%BD%D0%BD%D1%96%D0%B9%D0%93%D0%B5%D1%82%D1%8C%D0%BC%D0%B0%D0%BD",
                                        StreetcodeId = 1
                                    },
                                    new Video
                                    {
                                        Title = "Біографія Т.Г.Шевченка",
                                        Url = "https://www.youtube.com/watch?v=YuoaECXH2Bc&ab_channel=%D0%A2%D0%B2%D0%BE%D1%8F%D0%9F%D1%96%D0%B4%D0%BF%D1%96%D0%BB%D1%8C%D0%BD%D0%B0%D0%93%D1%83%D0%BC%D0%B0%D0%BD%D1%96%D1%82%D0%B0%D1%80%D0%BA%D0%B0",
                                        StreetcodeId = 2
                                    });

                                await dbContext.SaveChangesAsync();
                            }

                            if (!dbContext.Partners.Any())
                            {
                                dbContext.Partners.AddRange(
                                    new Partner
                                    {
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
                                        Title = "Parimatch",
                                        Description = "Конторка для лошків з казіничами та лохотроном, аби стягнути побільше бабок з довірливих дурбобиків",
                                        LogoId = 13,
                                        TargetUrl = "https://parimatch.com/"
                                    },
                                    new Partner
                                    {
                                        Title = "comunity partner",
                                        Description = "Класна платформа, я зацінив, а ти?",
                                        LogoId = 14,
                                        TargetUrl = "https://partners.salesforce.com/pdx/s/?language=en_US&redirected=RGSUDODQUL"
                                    });

                                await dbContext.SaveChangesAsync();

                                if (!dbContext.PartnerSourceLinks.Any())
                                {
                                    dbContext.PartnerSourceLinks.AddRange(
                                        new PartnerSourceLink
                                        {
                                            LogoType = LogoType.Twitter,
                                            TargetUrl = "https://twitter.com/SoftServeInc",
                                            PartnerId = 1
                                        },
                                        new PartnerSourceLink
                                        {
                                            LogoType = LogoType.Instagram,
                                            TargetUrl = "https://www.instagram.com/softserve_people/",
                                            PartnerId = 1
                                        },
                                        new PartnerSourceLink
                                        {
                                            LogoType = LogoType.Facebook,
                                            TargetUrl = "https://www.facebook.com/SoftServeCompany",
                                            PartnerId = 1
                                        });

                                    await dbContext.SaveChangesAsync();
                                }

                                if (!dbContext.StreetcodePartners.Any())
                                {
                                    dbContext.StreetcodePartners.AddRange(
                                        new StreetcodePartner
                                        {
                                            StreetcodeId = 2,
                                            PartnerId = 1
                                        },
                                        new StreetcodePartner
                                        {
                                            StreetcodeId = 2,
                                            PartnerId = 2
                                        },
                                        new StreetcodePartner
                                        {
                                            StreetcodeId = 2,
                                            PartnerId = 3
                                        },
                                        new StreetcodePartner
                                        {
                                            StreetcodeId = 1,
                                            PartnerId = 1
                                        },
                                        new StreetcodePartner
                                        {
                                            StreetcodeId = 1,
                                            PartnerId = 2
                                        },
                                        new StreetcodePartner
                                        {
                                            StreetcodeId = 1,
                                            PartnerId = 3
                                        });

                                    await dbContext.SaveChangesAsync();
                                }
                            }

                            if (!dbContext.Arts.Any())
                            {
                                dbContext.Arts.AddRange(
                                    new Art
                                    {
                                        ImageId = 19,
                                        Title = "Анатолій Федірко",
                                        Description = "Анатолій Федірко, «Український супрематичний політичний діяч Михайло Грушевський», 2019-2020 роки."
                                    },
                                    new Art
                                    {
                                        ImageId = 20,
                                        Title = "Анатолій Федірко",
                                        Description = "Анатолій Федірко, «Український супрематичний політичний діяч Михайло Грушевський», 2019-2020 роки."
                                    },
                                    new Art
                                    {
                                        ImageId = 21,
                                        Title = "Назар Дубів",
                                        Description = "Назар Дубів опублікував серію малюнків, у яких перетворив класиків української літератури та політичних діячів на сучасних модників"
                                    },
                                    new Art
                                    {
                                        ImageId = 22
                                    },
                                    new Art
                                    {
                                        ImageId = 22,
                                        Title = "Козаки на орбіті",
                                        Description = "«Козаки на орбіті» поєднує не тільки тему козаків, а й апелює до космічної тематики."
                                    },
                                    new Art
                                    {
                                        ImageId = 21,
                                        Title = "Січових стрільців",
                                        Description = "На вулиці Січових стрільців, 75 закінчили малювати мурал Михайла Грушевського на місці малюнка будинку з лелекою."
                                    },
                                    new Art
                                    {
                                        ImageId = 16,
                                        Title = "Січових стрільців",
                                        Description = "Some Description"
                                    },
                                    new Art
                                    {
                                        ImageId = 17,
                                        Title = "Січових стрільців",
                                        Description = "Some Description"
                                    },
                                    new Art
                                    {
                                        ImageId = 18,
                                        Title = "Січових стрільців",
                                        Description = "Some Description"
                                    },
                                    new Art
                                    {
                                        ImageId = 19,
                                        Title = "Січових стрільців",
                                        Description = "Some Description"
                                    });

                                await dbContext.SaveChangesAsync();

                                if (!dbContext.StreetcodeArts.Any())
                                {
                                    dbContext.StreetcodeArts.AddRange(
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
                                        },
                                        new StreetcodeArt
                                        {
                                            ArtId = 7,
                                            StreetcodeId = 2,
                                            Index = 1,
                                        },
                                        new StreetcodeArt
                                        {
                                            ArtId = 4,
                                            StreetcodeId = 2,
                                            Index = 2,
                                        },
                                        new StreetcodeArt
                                        {
                                            ArtId = 5,
                                            StreetcodeId = 2,
                                            Index = 3,
                                        },
                                        new StreetcodeArt
                                        {
                                            ArtId = 6,
                                            StreetcodeId = 2,
                                            Index = 4,
                                        });

                                    await dbContext.SaveChangesAsync();
                                }
                            }

                            if (!dbContext.Texts.Any())
                            {
                                dbContext.Texts.AddRange(
                                    new Text
                                    {
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
                                        Title = "Бунтар чи громадянин доброї волі?",
                                        TextContent = "Юний, харизматичний. Громадський активіст родом з Києва, а духом точно з якоїсь козацької колиски на кшталт Холодного Яру. З його доброї волі йому вдавалося все чи майже все, за що він брався за свої неповні 25. Для багатьох Роман Ратушний втілював надію на краще майбутнє та результативне лідерство. Надію на подолання корупції, лідерство в протистоянні незаконній забудові законними методами. \r\n \r\nВін був бунтарем. Але йшлося не про юнацький максималізм чи протест заради протесту. Роман бунтував проти несправедливості. Мафіозна дійсність, корупція та свавілля влади, окупант на твоїй землі. Захистити історичну спадщину чи згуртувати потужну громаду — таким було громадянське лицарство Ратушного.\r\n\r\nРідний Київ хлопець обожнював. Україну щиро любив. І з візій кращого майбутнього постійно народжувалися різні проєкти та ініціативи. Зокрема, Роман активно виступав за дерусифікацію: «Випалюйте в собі всю російську субкультуру. Інакше це все випалить вас».\r\n\r\nСаме в любові до Києва Роман ініціював та створив ГО «Захистимо Протасів Яр». Усе починалося як протест місцевої громади проти побудови на історичних схилах 40-поверхівок. А переросло в об’єднання, яке захищає права киян. І виграє суди у великих заангажованих бізнес-структур. Так сталося в 2021 році, коли Господарський суд визнав недійсним договір суборенди земельної ділянки. А мер Києва пізніше підтвердив наміри створити тут парк. Парк, де на честь Роми пообіцяли висадити дуби його колеги та друзі. \r\n\r\nПомаранчева революція, Революція гідності, боротьба із незаконною забудовою у Протасовому Яру. Юнак брав участь практично в усіх великих заходах, мітингах, акціях, де йшлося про боротьбу за справедливість. За словами батьків Романа, вони не виховували активного громадянина навмисне, він таким народився. «Думаю, такі люди з’являються на світ уже абсолютно досконалими. Про Рому я це відчувала і знала одразу. Це не є результат якогось виховання. Це абсолютно сформована особистість надзвичайно високого рівня. В усіх сенсах», — так сказала про сина письменниця Світлана Поваляєва.\r\n\r\nРоман змінив життя багатьох людей. Заради справедливості був готовий іти до кінця. Так, тато Тарас Ратушний про свого Романа каже: «Різниця між нами, моїм поколінням і нашими дітьми, в тому, що вони не зупиняються. Я не впевнений, усвідомлюють вони це чи ні, але якщо зважити ризики, якщо подумати, що станеться, можна програти. Тому треба діяти тут і зараз, до кінця. Ось де різниця. Ось про що Роман». Як і Роман, його батько приєднався до лав ЗСУ.\r\n\r\nПерші великі гроші юнак заробив завдяки тому, що обробляв мемуари відомої єврейської діячки. А до ГО «Захистимо Протасів Яр» якийсь час працював у комітеті Верховної Ради з питань житлово-комунальних послуг. І це йому, молодому студенту-правнику, було цікаво. Історія та право взагалі були на першому місці серед інтересів хлопця.\r\n\r\nВосени 2020 року Роман висунув свою кандидатуру на депутатство в Київраді, і хоч не пройшов тоді, але не засмутився. Це був його політичний досвід: зустрічався з виборцями, радив об’єднуватися в громади, закликав домовлятися одне з одним, щоб робити добрі справи. Роману вдавалося переконувати людей щирим словом.\r\n\r\nВін був різноплановою особистістю, швидко все опановував. За словами мами, рано почав ходити, швидко навчився говорити. А ще в його житті було багато музики, від фольклору до рок-н-ролу. Навчався в Джазовій академії Басюків на Оболоні разом зі старшим братом Василем, який у 2014 році, із початком російсько-української війни, поповнив лави ЗСУ. \r\n\r\n«Я знала Рому по всіх, напевно, найгучніших, найважливіших і найрезонансніших ініціативах, які сталися за останні кілька років. Він був прикладом і натхненням для чималого покоління, особливо — для молодих українців та українок», — каже активістка Марина Хромих, підкреслюючи масштабність Романа як людини. \r\n\r\nНа думку журналіста Дениса Казанського, Роман Ратушний був одним із найефективніших представників громадського сектору Києва. «Коли ми познайомилися, йому було 21–22 роки. Він надихав. Я вірив, що в нього велике політичне майбутнє. Радів, що в нас є такі люди».\r\n\r\nУ березні 2021 року Роман Ратушний разом з багатьма небайдужими мітингував проти незаконного увʼязнення Сергія Стерненка, одеського активіста. Побиті вікна Офісу Президента, розмальований фасад. Було сфабриковано відео, де це робить начебто Роман. Після публікації відео на сайті МВС Ратушного затримали та інкримінували групове хуліганство. В результаті — домашній арешт з електронним браслетом. Ратушний пов’язував таке рішення із «політичною» неприязню до нього з боку апарату Офісу Президента. За місяць завдяки зусиллям адвокатів Романа апеляційний суд зняв з нього всі обвинувачення.\r\n\r\nВипадків погроз про фізичну розправу над активістом Ратушним було безліч. Сам Роман пов’язував їх зі своєю діяльністю щодо захисту Протасового Яру та, зокрема, з компанією-забудовницею та особами-бенефіціарами. Так, хлопця намагалися страхати навіть відправкою на фронт за активну громадянську позицію. А він завжди відповідав у своїх численних інтерв’ю ЗМІ, що для нього захист Батьківщини не є покаранням.\r\n\r\nІ він пішов її захищати. З перших днів повномасштабного вторгнення. Думав про це й раніше, бо мав приклад брата. Спочатку служив у підрозділі «Протасового Яру» в обороні Києва. Згодом приєднався разом з кількома бойовими побратимами до 93-ї бригади на півночі Сумської області. Брав участь у деокупації Тростянця. Назва бригади — «Холодний Яр» — була натхненням для Романа, як історична пам’ять про опір українців загарбникам.\r\n\r\nПопри невеликий бойовий досвід, Ратушний став розвідником. Це одна з найнебезпечніших спеціалізацій через наближення впритул до ворога. 7 квітня 2022 року Роман опублікував на своїй фейсбук-сторінці військовий квиток як «план до кінця війни». Потім був Ізюмський напрямок. Через зв’язки в Києві постійно підбирав машини та обладнання для батальйону.\r\n\r\nУ своє останнє бойове завдання Роман підповз до позицій росіян і визначив розташування їхніх танків. Зміг розмінувати дорогу, але ворог його помітив. 9 червня 2022 року Роман Ратушний з позивним Сенека загинув у складі бойової групи. До останнього подиху Роман був «на самому вістрі. І навіть ще трішки попереду».\r\n\r\n«Рома не хотів би, щоб ми плакали. Він хотів би, щоб ми перемогли», — сказала мама Романа. А ще її син хотів, щоб на його могилі поставили козацький хрест, а на ньому як епітафію вибили вірш Михайля Семенка «Патагонія»:\r\n\r\nЯ не умру від смерти — \r\nЯ умру від життя. \r\nУмиратиму — життя буде мерти, \r\nНе маятиме стяг.\r\n\r\nЯ молодим, молодим умру —\r\nБо чи стану коли старим? \r\nЗалиш, залиш траурну гру. \r\nРозсип похоронні рими.\r\n\r\nЯ умру, умру в Патагонії дикій, \r\nБо належу огню й землі. \r\nРідні мої, я не чутиму ваших криків, \r\nЯ — нічий, поет світових слів.\r\n\r\nЯ умру в хвилю, коли природа стихне, \r\nЧекаючи на останню горобину ніч. \r\nЯ умру в павзу, коли серце стисне \r\nМоя молодість, і життя, і січа.\r\n",
                                        StreetcodeId = 2
                                    });

                                await dbContext.SaveChangesAsync();
                            }

                            if (!dbContext.TimelineItems.Any())
                            {
                                dbContext.TimelineItems.AddRange(
                                    new TimelineItem
                                    {
                                        Date = new DateTime(1831, 1, 1),
                                        Title = "Перші роки в Петербурзі",
                                        Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                                    "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                                    " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.",
                                        StreetcodeId = 1
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(1830, 1, 1),
                                        Title = "Учень Петербурзької академії мистецтв",
                                        Description = "Засвідчивши свою відпускну в петербурзькій Палаті цивільного суду, Шевченко став учнем Академії мистецтв," +
                                            " де його наставником став К. Брюллов. За словами Шевченка: «настала найсвітліша доба його життя, незабутні, золоті дні»" +
                                            " навчання в Академії мистецтв, яким він присвятив у 1856 році автобіографічну повість «Художник».",
                                        StreetcodeId = 1
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(1832, 1, 1),
                                        Title = "Перші роки в Петербурзі",
                                        Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                                                    "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                                                    " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.",
                                        StreetcodeId = 1
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(1833, 1, 1),
                                        Title = "Перші роки в Петербурзі",
                                        Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                                                    "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                                                    " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.",
                                        StreetcodeId = 1
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(1834, 1, 1),
                                        Title = "Перші роки в Петербурзі",
                                        Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                                                    "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                                                    " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.",
                                        StreetcodeId = 1
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(1835, 1, 1),
                                        Title = "Перші роки в Петербурзі",
                                        Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                                                    "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                                                    " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.",
                                        StreetcodeId = 1
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(1836, 1, 1),
                                        Title = "Перші роки в Петербурзі",
                                        Description = "Переїхавши 1831 року з Вільна до Петербурга, поміщик П. Енгельгардт узяв із собою Шевченка, " +
                                                    "а щоб згодом мати зиск на художніх творах власного «покоєвого художника», підписав контракт й віддав його" +
                                                    " в науку на чотири роки до живописця В. Ширяєва, у якого й замешкав Тарас до 1838 року.",
                                        StreetcodeId = 1
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(1997, 7, 5),
                                        DateViewPattern = DateViewPattern.DateMonthYear,
                                        Title = "Народився",
                                        Description = "Цього дня Роман народився в Києві. В родині активіста руху проти знищення історичної забудови «Збережи старий Київ», добровольця Тараса Ратушного та письменниці, журналістки Світлани Поваляєвої. Зростав та вчився у столиці.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2012, 1, 1),
                                        Title = "Обирає фах",
                                        DateViewPattern = DateViewPattern.Year,
                                        Description = "Коли прийшов час обирати фах, Роман зупиняє свій вибір на юридичному та вступає до Фінансово-правового коледжу в Києві.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2013, 11, 30),
                                        DateViewPattern = DateViewPattern.DateMonthYear,
                                        Title = "Проти несправедливості",
                                        Description = "Роману — 15. Україні — 22. І обох непокоїть несправедливість. Починається Революція гідності. Юний Ратушний — один з перших її учасників в усіх найгарячіших епізодах протистояння. У ніч на 30 листопада його разом з іншими студентами вперше побив «Беркут».",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2013, 12, 30),
                                        DateViewPattern = DateViewPattern.MonthYear,
                                        Title = "«Знаю, що роблю»",
                                        Description = "Під час штурму Євромайдану силовиками Роман хоч і постраждав, але вистояв разом з іншими гідними. «Тато. Знаю, що я роблю», — спокійно відповідає батькові та йде туди, де найгарячіше.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2014, 1, 1),
                                        DateViewPattern = DateViewPattern.Year,
                                        Title = "Боротьба лише починається",
                                        Description = "Найзапекліша фаза Революції гідності у лютому. Силовий тиск проти активістів поновлюється. Роман знову в епіцентрі. Його боротьба тільки починається. У грудні бере активну участь у протестах за кадрові зміни в Міністерстві внутрішніх справ України та пришвидшення розслідувань злочинів, скоєних у 2013–2014 роках на Євромайдані та в Одесі.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2018, 1, 1),
                                        DateViewPattern = DateViewPattern.Year,
                                        Title = "Захистимо Протасів Яр",
                                        Description = "Роман очолює ініціативу «Захистимо Протасів Яр», з 2019 року це однойменна громадська організація. Разом з однодумцями активно виступає за збереження зеленої зони у Протасовому Яру в центрі Києва та проти побудови багатоповерхівок на зелених схилах.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2019, 1, 1),
                                        DateViewPattern = DateViewPattern.Year,
                                        Title = "Погрози",
                                        Description = "Через погрози фізичною розправою та викраденням, про які Роман заявив у жовтні 2019-го, йому доводиться деякий час переховуватися.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2020, 6, 1),
                                        DateViewPattern = DateViewPattern.SeasonYear,
                                        Title = "Перемога в суді",
                                        Description = "Конфлікт та протистояння забудовнику ТОВ «Дайтона Груп» в суді нарешті закінчуються перемогою активістів на чолі з Ратушним. 27 червня 2020 року Київська міська рада повертає земельній ділянці площею 3,25 га у Протасовому Яру статус зелених насаджень.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2020, 12, 30),
                                        DateViewPattern = DateViewPattern.SeasonYear,
                                        Title = "Досвід політика",
                                        Description = "Роман балотується на виборах депутатів Київради від блоку Віталія Кличка, не будучи членом партії «УДАР». На думку членів ГО «Захистимо Протасів Яр», така взаємодія мала б забезпечити представництво в міській раді громадських ініціатив, а не тільки партій. Вибори Роман програє, не подолавши 25-відсоткової виборчої квоти, але набувши певного досвіду політика.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2021, 1, 1),
                                        DateViewPattern = DateViewPattern.Year,
                                        Title = "Домашній арешт",
                                        Description = "Роман активно підтримує виступи проти арештів активістів: одесита Стерненка та затриманих у «справі Шеремета» Антоненка, Дугарь, Кузьменко. Проти нього фабрикують справу. У соцмережах її охрестили «чорний квадрат» через суцільну чорну пляму на відео з камер, в якому нібито побачили Романа. На підставі сфабрикованих доказів висувають підозру в хуліганстві та відправляють під домашній арешт.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2021, 1, 1),
                                        DateViewPattern = DateViewPattern.Year,
                                        Title = "Сфабрикована справа",
                                        Description = "Громадську діяльність під домашнім арештом не полишає. Знімає жартівливе відео про життя з електронним браслетом. Свій арешт пов’язує з власною діяльністю на захист Протасового Яру. «Навіть якби мене на тій акції не було, вони б придумали щось інше», — каже про фабрикування справи. Після подання апеляції адвокати активіста виграли суд — з Ратушного зняли всі обвинувачення.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2022, 2, 24),
                                        DateViewPattern = DateViewPattern.DateMonthYear,
                                        Title = "Підрозділ Протасового",
                                        Description = "Свій Протасів та свій Київ з початком повномасштабного вторгнення Росії Роман добровольцем захищає у лавах Збройних сил України в підрозділі «Протасового Яру». Спершу була Київщина, потім — Сумщина, де він брав участь у деокупації населених пунктів області, зокрема Тростянця.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2022, 3, 1),
                                        DateViewPattern = DateViewPattern.SeasonYear,
                                        Title = "Холодний Яр",
                                        Description = "На початку квітня вступає до розвідувального взводу 2-го мотопіхотного батальйону 93-ї окремої механізованої бригади ЗСУ «Холодний Яр». Цей батальйон обороняв українську землю в Харківській області, зокрема в районі Ізюму.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2022, 6, 9),
                                        DateViewPattern = DateViewPattern.DateMonthYear,
                                        Title = "Завжди 24",
                                        Description = "Роман Ратушний не дожив трохи менше місяця до своїх 25 років. 9 червня 2022-го під Ізюмом на Харківщині він загинув, потрапивши у ворожу засідку. До кінця на бойовому завданні, вистежуючи ворожий танк. Тіло Романа декілька днів було на непідконтрольній території, доки його командир з позивним Боб зміг його забрати. Чекав сильної грози, щоб не бути поміченим ворогом.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2022, 6, 18),
                                        DateViewPattern = DateViewPattern.DateMonthYear,
                                        Title = "Байкове. Вічність",
                                        Description = "Романа Ратушного поховали на Байковому кладовищі в Києві. Попрощатися прийшли сотні людей. Батьки, родичі, військові, знайомі та друзі Романа, громадяни, активісти, представники влади. Прощалися з героєм у Михайлівському соборі та на Майдані. Перед похороном над труною з його тілом розгорнули прапор України.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2022, 9, 8),
                                        DateViewPattern = DateViewPattern.DateMonthYear,
                                        Title = "Вулиця Ратушного",
                                        Description = "На засіданні Київради одноголосно підтримали перейменування вулиці Волгоградської у Солом'янському районі столиці на вулицю Романа Ратушного. Таку пропозицію подав письменник Євген Лір.",
                                        StreetcodeId = 2
                                    },
                                    new TimelineItem
                                    {
                                        Date = new DateTime(2022, 9, 13),
                                        DateViewPattern = DateViewPattern.DateMonthYear,
                                        Title = "За мужність",
                                        Description = "Романа Ратушного посмертно нагородили орденом «За мужність» III ступеня — за особисту мужність і самовіддані дії, виявлені у захисті державного суверенітету та територіальної цілісності України, вірність військовій присязі.",
                                        StreetcodeId = 2
                                    });

                                await dbContext.SaveChangesAsync();

                                if (!dbContext.HistoricalContexts.Any())
                                {
                                    dbContext.HistoricalContexts.AddRange(
                                        new HistoricalContext
                                        {
                                            Title = "Дитинство"
                                        },
                                        new HistoricalContext
                                        {
                                            Title = "Студентство"
                                        },
                                        new HistoricalContext
                                        {
                                            Title = "Життя в Петербурзі"
                                        },
                                        new HistoricalContext
                                        {
                                            Title = "Незалежна Україна"
                                        },
                                        new HistoricalContext
                                        {
                                            Title = "Революція гідності"
                                        },
                                        new HistoricalContext
                                        {
                                            Title = "Збройна агресія Росії"
                                        },
                                        new HistoricalContext
                                        {
                                            Title = "Повномасштабне вторгнення Росії"
                                        });

                                    await dbContext.SaveChangesAsync();

                                    if (!dbContext.HistoricalContextsTimelines.Any())
                                    {
                                        dbContext.HistoricalContextsTimelines.AddRange(
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 3,
                                            TimelineId = 1
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 2,
                                            TimelineId = 2,
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 3,
                                            TimelineId = 3
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 3,
                                            TimelineId = 4
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 3,
                                            TimelineId = 5
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 3,
                                            TimelineId = 6
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 3,
                                            TimelineId = 7
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 4,
                                            TimelineId = 8
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 4,
                                            TimelineId = 9
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 5,
                                            TimelineId = 10
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 5,
                                            TimelineId = 11
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 6,
                                            TimelineId = 12
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 6,
                                            TimelineId = 13
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 6,
                                            TimelineId = 14
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 6,
                                            TimelineId = 15
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 6,
                                            TimelineId = 16
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 6,
                                            TimelineId = 17
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 6,
                                            TimelineId = 18
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 7,
                                            TimelineId = 19
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 7,
                                            TimelineId = 20
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 7,
                                            TimelineId = 21
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 7,
                                            TimelineId = 22
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 7,
                                            TimelineId = 23
                                        },
                                        new HistoricalContextTimeline
                                        {
                                            HistoricalContextId = 7,
                                            TimelineId = 24
                                        });

                                        await dbContext.SaveChangesAsync();
                                    }
                                }
                            }

                            if (!dbContext.TransactionLinks.Any())
                            {
                                dbContext.TransactionLinks.AddRange(
                                    new TransactionLink
                                    {
                                        Url = "https://streetcode/1",
                                        StreetcodeId = 1
                                    },
                                    new TransactionLink
                                    {
                                        Url = "https://streetcode/2",
                                        StreetcodeId = 2
                                    });

                                await dbContext.SaveChangesAsync();
                            }

                            if (!dbContext.Facts.Any())
                            {
                                dbContext.Facts.AddRange(
                                    new Fact
                                    {
                                        Title = "Викуп з кріпацтва",
                                        FactContent = "Навесні 1838-го Карл Брюллов і Василь Жуковський вирішили викупити молодого поета з кріпацтва. " +
                                        "Енгельгардт погодився відпустити кріпака за великі гроші — 2500 рублів. Щоб здобути такі гроші, Карл Брюллов" +
                                        " намалював портрет Василя Жуковського — вихователя спадкоємця престолу Олександра Миколайовича, і портрет розіграли" +
                                        " в лотереї, у якій взяла участь імператорська родина. Лотерея відбулася 4 травня 1838 року," +
                                        " а 7 травня Шевченкові видали відпускну.",
                                        ImageId = 6,
                                        StreetcodeId = 1,
                                    },
                                    new Fact
                                    {
                                        Title = "Перший Кобзар",
                                        FactContent = " Ознайомившись випадково з рукописними творами Шевченка й вражений ними, П. Мартос виявив до них великий інтерес." +
                                            " Він порадився із Є. Гребінкою і запропонував Шевченку видати їх окремою книжкою, яку згодом назвали «Кобзарем».",
                                        ImageId = 5,
                                        StreetcodeId = 1,
                                    },
                                    new Fact
                                    {
                                        Title = "Премія Романа Ратушного",
                                        FactContent = "Український журналіст, публіцист і письменник Вахтанг Кіпіані від імені «Історичної правди» ініціював заснування іменної премії Романа Ратушного для молодих авторів за публікації, що стосуються історії Києва. Гроші на започаткування премії дали батьки Романа.",
                                        ImageId = 16,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Стипендія для активістів",
                                        FactContent = "На честь Романа в Інституті права Київського національного університету імені Тараса Шевченка заснували стипендіальну програму для громадських активістів, які здобувають юридичну освіту.",
                                        ImageId = 17,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Премія Романа Ратушного",
                                        FactContent = "Український журналіст, публіцист і письменник Вахтанг Кіпіані від імені «Історичної правди» ініціював заснування іменної премії Романа Ратушного для молодих авторів за публікації, що стосуються історії Києва. Гроші на започаткування премії дали батьки Романа.",
                                        ImageId = 18,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Карта мафіозних зв’язків",
                                        FactContent = "Романа можна вважати «хрещеним» Державного бюро розслідувань. У 2015 році він самостійно створив карту зв’язків російської та української мафій, засновану на відкритих даних. Підтримував розслідування злочинів. За його даними, таких взаємопов’язаних осіб було близько тисячі.",
                                        ImageId = 19,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Стати громадянином",
                                        FactContent = "За словами мами Романа, Світлани Поваляєвої, маленький громадянин Ратушний почав ходити на мітинги та протести із семи років. Першою суспільно корисною активністю стала Помаранчева революція. «Участь у політичних і соціальних процесах своєї держави має бути атрибутом життя кожного громадянина», — наголошував Роман.",
                                        ImageId = 16,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "«У мене все добре»",
                                        FactContent = "Якось під час найзапеклішого протистояння на Євромайдані він подзвонив батькові та звично сказав: «У мене все добре, ми з друзями вже їдемо з Майдану додому, не хвилюйся і на добраніч». А через деякий час в той же вечір Роман вже коментував події у прямій телевізійній трансляції: «Ми зараз штурмуємо Український Дім, там засіли внутрішні війська, їх біля сотні, але ми їх зараз звідти викуримо…». Він завжди був там, де найгарячіше. Каску, в якій Роман був на Майдані, його мама Світлана Поваляєва згодом передала в Музей Революції Гідності.",
                                        ImageId = 17,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Життєві плани",
                                        FactContent = "Після Революції гідності в 2014 році під час подорожі Європою тато Романа делікатно просував йому, юнакові, обпаленому Майданом, ідею навчання в одному з європейських університетів. А Роман делікатно відмовився. «На цей момент мій життєвий план такого не передбачає», — відповів.",
                                        ImageId = 18,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Культура життя",
                                        FactContent = "Роман зростав у середовищі культурних діячів Києва. Це не могло не позначитися на його особистості, поглядах і смаках. Театри, вистави, виставки. Багато музики та читання. Цікавість до історії та права. Разом із братом Василем навчався грі на трубі в Джазовій академії. Відвідував концерти в Національній філармонії та Будинку органної і камерної музики. Друзі відзначали витончений смак Романа в одязі, але в цілому аскетичний підхід до матеріальних принад життя.",
                                        ImageId = 19,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Громада понад усе",
                                        FactContent = "Всі знали Ратушного як щедру та безкорисливу людину. Так, компенсацію Європейського суду з прав людини, яку він отримав як потерпілий від побиття студентів «Беркутом», Роман фактично повністю витратив на громаду та боротьбу за Протасів Яр.",
                                        ImageId = 16,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Сенека",
                                        FactContent = "На фронті Роман взяв собі позивний Сенека. Йому відгукувалися погляди давньоримського філософа на жертовність та героїзм заради суспільства. Сенека виклав їх у своїх «Листах». А Роман підтвердив свої переконання яскравим життям із героїчним запалом. Зі світоглядом Сенеки погляди Романа порівняв його батько в своєму тексті пам’яті про сина. Спецпідрозділ радіоелектронної розвідки і радіоелектронного штурму 93-ї окремої механізованої бригади ЗСУ «Холодний Яр», де служив Ратушний, назвали «Сенека». На його честь, бо він його і задумав.",
                                        ImageId = 17,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Заповіт нам",
                                        FactContent = "20 травня 2022 року, незадовго до загибелі, Роман на своїй сторінці у фейсбуці опублікував пост — свого роду заповіт нам. «Допоки Збройні сили вбивають русню на фронті, ви нездатні вбити русню в собі. Просто запам’ятайте: чим більше росіян ми вб’ємо зараз, тим менше росіян доведеться вбивати нашим дітям. Ця війна триває більше трьох сотень років…».",
                                        ImageId = 18,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "«Загинув за Тебе»",
                                        FactContent = "Побратими стверджують, що Роман готувався до свого останнього бойового завдання. Дав вказівку зібрати свої речі та віддати їх братові у випадку загибелі. Написав заповіт з докладними інструкціями. Описав, як саме хотів провести свій похорон. А ще написав у заповіті кілька останніх слів про любов до свого Києва: «Загинув далеко від Тебе, Києве, але загинув за Тебе…».",
                                        ImageId = 19,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Мріяв про вільну Україну",
                                        FactContent = "У липні 2022 року в Лугано президентка Єврокомісії Урсула фон дер Ляєн під час конференції з відбудови України розпочала свій виступ словами про Романа, активіста та журналіста, який мріяв про Україну, вільну від корупції, та поклав життя за її суверенітет.",
                                        ImageId = 16,
                                        StreetcodeId = 2
                                    },
                                    new Fact
                                    {
                                        Title = "Жива справа",
                                        FactContent = "За словами мами Романа Світлани Поваляєвої, він заповів фінансово підтримати музей Шевченка, Національну капелу бандуристів імені Майбороди, а також видання «Історична правда» та «Новинарня». А ще попросив донатити на добровольчий медичний батальйон «Госпітальєри» та інші волонтерські організації, що займаються екіпіруванням ЗСУ.",
                                        ImageId = 17,
                                        StreetcodeId = 2
                                    });
                                await dbContext.SaveChangesAsync();
                                dbContext.ImageDetailses.AddRange(new[]
                                {
                                     new ImageDetails()
                                     {
                                         ImageId = 6,
                                         Alt = "Additional inforamtaion for  wow-fact photo 1"
                                     },
                                     new ImageDetails()
                                     {
                                         ImageId = 16,
                                         Alt = "Additional inforamtaion for  wow-fact photo 2"
                                     },
                                     new ImageDetails()
                                     {
                                         ImageId = 17,
                                         Alt = "Additional inforamtaion for  wow-fact photo 3"
                                     },
                                     new ImageDetails()
                                     {
                                         ImageId = 19,
                                         Alt = "Additional inforamtaion for  wow-fact photo 3"
                                     },
                                });
                            }

                            if (!dbContext.SourceLinks.Any())
                            {
                                dbContext.SourceLinks.AddRange(
                                    new SourceLinkCategory
                                    {
                                        Title = "Книги",
                                        ImageId = 9,
                                    },
                                    new SourceLinkCategory
                                    {
                                        Title = "Фільми",
                                        ImageId = 10,
                                    },
                                    new SourceLinkCategory
                                    {
                                        Title = "Цитати",
                                        ImageId = 11,
                                    });

                                await dbContext.SaveChangesAsync();

                                if (!dbContext.StreetcodeCategoryContent.Any())
                                {
                                    dbContext.StreetcodeCategoryContent.AddRange(
                                        new StreetcodeCategoryContent
                                        {
                                            Text = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                                            SourceLinkCategoryId = 1,
                                            StreetcodeId = 2
                                        },
                                        new StreetcodeCategoryContent
                                        {
                                            Text = "Хроніки про Т. Г. Шевченко",
                                            SourceLinkCategoryId = 2,
                                            StreetcodeId = 2
                                        },
                                        new StreetcodeCategoryContent
                                        {
                                            Text = "Цитати про Шевченка",
                                            SourceLinkCategoryId = 3,
                                            StreetcodeId = 2
                                        },
                                        new StreetcodeCategoryContent
                                        {
                                            Text = "Пряма мова",
                                            SourceLinkCategoryId = 3,
                                            StreetcodeId = 1
                                        });

                                    await dbContext.SaveChangesAsync();
                                }
                            }

                            if (!dbContext.RelatedFigures.Any())
                            {
                                dbContext.RelatedFigures.AddRange(
                                    new RelatedFigure
                                    {
                                        ObserverId = 2,
                                        TargetId = 1
                                    },
                                    new RelatedFigure
                                    {
                                        ObserverId = 1,
                                        TargetId = 2
                                    });

                                await dbContext.SaveChangesAsync();
                            }

                            if (!dbContext.StreetcodeImages.Any())
                            {
                                dbContext.StreetcodeImages.AddRange(
                                    new StreetcodeImage
                                    {
                                        ImageId = 1,
                                        StreetcodeId = 1,
                                    },
                                    new StreetcodeImage
                                    {
                                        ImageId = 5,
                                        StreetcodeId = 1,
                                    },
                                    new StreetcodeImage
                                    {
                                        ImageId = 1,
                                        StreetcodeId = 2,
                                    },
                                    new StreetcodeImage
                                    {
                                        ImageId = 23,
                                        StreetcodeId = 2,
                                    });

                                await dbContext.SaveChangesAsync();
                            }

                            if (!dbContext.Tags.Any())
                            {
                                dbContext.Tags.AddRange(
                                    new Tag
                                    {
                                        Title = "writer"
                                    },
                                    new Tag
                                    {
                                        Title = "artist"
                                    },
                                    new Tag
                                    {
                                        Title = "composer"
                                    },
                                    new Tag
                                    {
                                        Title = "victory"
                                    },
                                    new Tag
                                    {
                                        Title = "Наукова школа"
                                    },
                                    new Tag
                                    {
                                        Title = "Історія"
                                    },
                                    new Tag
                                    {
                                        Title = "Політика"
                                    },
                                    new Tag
                                    {
                                        Title = "Активіст",
                                    },
                                    new Tag
                                    {
                                        Title = "Борці за незалежність",
                                    },
                                    new Tag
                                    {
                                        Title = "Герої",
                                    });

                                await dbContext.SaveChangesAsync();

                                if (!dbContext.StreetcodeTagIndices.Any())
                                {
                                    dbContext.StreetcodeTagIndices.AddRange(
                                        new StreetcodeTagIndex
                                        {
                                            TagId = 1,
                                            StreetcodeId = 1,
                                            IsVisible = true,
                                        },
                                        new StreetcodeTagIndex
                                        {
                                            TagId = 1,
                                            StreetcodeId = 2,
                                            IsVisible = true,
                                        },
                                        new StreetcodeTagIndex
                                        {
                                            TagId = 2,
                                            StreetcodeId = 1,
                                            IsVisible = true,
                                        },
                                        new StreetcodeTagIndex
                                        {
                                            TagId = 4,
                                            StreetcodeId = 2,
                                            IsVisible = true,
                                        },
                                        new StreetcodeTagIndex
                                        {
                                            TagId = 7,
                                            StreetcodeId = 2,
                                            IsVisible = true,
                                        },
                                        new StreetcodeTagIndex
                                        {
                                            TagId = 8,
                                            StreetcodeId = 2,
                                            IsVisible = true,
                                        },
                                        new StreetcodeTagIndex
                                        {
                                            TagId = 9,
                                            StreetcodeId = 2,
                                            IsVisible = true,
                                        },
                                        new StreetcodeTagIndex
                                        {
                                            TagId = 10,
                                            StreetcodeId = 2,
                                            IsVisible = true,
                                        });

                                    await dbContext.SaveChangesAsync();
                                }
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
