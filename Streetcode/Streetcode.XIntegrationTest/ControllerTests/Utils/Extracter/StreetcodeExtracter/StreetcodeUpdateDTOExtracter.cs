using System.Diagnostics.CodeAnalysis;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.DTO.Sources.Update;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Enums;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter
{
    [SuppressMessage(
        "Minor Code Smell",
        "S101:Types should be named in PascalCase",
        Justification = "DTOs are named this way throughout the project")]
    public static class StreetcodeUpdateDtoExtracter
    {
        private static SqlDbHelper dbHelper;

        static StreetcodeUpdateDtoExtracter()
        {
            dbHelper = BaseControllerTests.GetSqlDbHelper();
        }

        public static StreetcodeUpdateDto Extract(int id, int index, string transliterationUrl)
        {
            StreetcodeContent testStreetcode = StreetcodeContentExtracter.Extract(id, index, transliterationUrl);
            Image testImage = ImageExtracter.Extract(id);
            return GetTestStreetcodeUpdateDTO(testStreetcode.Id, testStreetcode.Index, testStreetcode.TransliterationUrl!, testImage);
        }

        public static void Remove(StreetcodeUpdateDto entity)
        {
            BaseExtracter.RemoveByPredicate<StreetcodeContent>(strCont => strCont.Id == entity.Id);

            foreach (var image in entity.Images)
            {
                var imageBlob = dbHelper.GetExistItemId<Image>(image.Id);
                ImageExtracter.Remove(imageBlob!);
            }

            foreach (var imageDetails in entity.ImagesDetails!)
            {
                BaseExtracter.RemoveById<ImageDetails>(imageDetails.Id);
            }
        }

        private static StreetcodeUpdateDto GetTestStreetcodeUpdateDTO(int id, int index, string transliterationUrl, Image testImage)
        {
            return new StreetcodeUpdateDto
            {
                Id = id,
                Index = index,
                TransliterationUrl = transliterationUrl,
                Title = "Test_Title",
                DateString = "тест-2024",
                Teaser = "Test_Teaser",
                Tags = new List<StreetcodeTagUpdateDto>(),
                Facts = new List<StreetcodeFactUpdateDto>(),
                Audios = new List<AudioUpdateDto>(),
                Images = new List<ImageUpdateDto>
                {
                    new ()
                    {
                        Id = testImage.Id,
                        ModelState = ModelState.Updated,
                        StreetcodeId = id,
                    },
                },
                ImagesDetails = new List<ImageDetailsDto>
                {
                    new ()
                    {
                        Alt = "1",
                        ImageId = testImage.Id,
                        Title = "test image",
                        Id = 0,
                    },
                },
                Videos = new List<VideoUpdateDto>(),
                Partners = new List<PartnersUpdateDto>(),
                Toponyms = new List<StreetcodeToponymCreateUpdateDto>(),
                Subtitles = new List<SubtitleUpdateDto>(),
                TimelineItems = new List<TimelineItemCreateUpdateDto>(),
                RelatedFigures = new List<RelatedFigureUpdateDto>(),
                Arts = new List<ArtCreateUpdateDto>(),
                StreetcodeArtSlides = new List<StreetcodeArtSlideCreateUpdateDto>(),
                StatisticRecords = new List<StatisticRecordUpdateDto>(),
                StreetcodeCategoryContents = new List<StreetcodeCategoryContentUpdateDto>(),
                ARBlockUrl = "https://streetcode/1"
            };
        }
    }
}
