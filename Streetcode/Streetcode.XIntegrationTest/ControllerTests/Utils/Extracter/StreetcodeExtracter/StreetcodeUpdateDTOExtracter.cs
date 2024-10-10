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
    public class StreetcodeUpdateDTOExtracter
    {
        private static SqlDbHelper _dbHelper;

        static StreetcodeUpdateDTOExtracter()
        {
            _dbHelper = BaseControllerTests.GetSqlDbHelper();
        }

        public static StreetcodeUpdateDTO Extract(int id, int index, string transliterationUrl)
        {
            StreetcodeContent testStreetcode = StreetcodeContentExtracter.Extract(id, index, transliterationUrl);
            Image testImage = ImageExtracter.Extract(id);
            return GetTestStreetcodeUpdateDTO(testStreetcode.Id, testStreetcode.Index, testStreetcode.TransliterationUrl, testImage);
        }

        public static void Remove(StreetcodeUpdateDTO entity)
        {
            BaseExtracter.RemoveByPredicate<StreetcodeContent>(strCont => strCont.Id == entity.Id);

            foreach (var image in entity.Images)
            {
                var imageBlob = _dbHelper.GetExistItemId<Image>(image.Id);
                ImageExtracter.Remove(imageBlob);
            }

            foreach (var imageDetails in entity.ImagesDetails)
            {
                BaseExtracter.RemoveById<ImageDetails>(imageDetails.Id);
            }
        }

        private static StreetcodeUpdateDTO GetTestStreetcodeUpdateDTO(int id, int index, string transliterationUrl, Image testImage)
        {
            return new StreetcodeUpdateDTO
            {
                Id = id,
                Index = index,
                TransliterationUrl = transliterationUrl,
                Title = "Test_Title",
                DateString = "тест-2024",
                Teaser = "Test_Teaser",
                Tags = new List<StreetcodeTagUpdateDTO>(),
                Facts = new List<StreetcodeFactUpdateDTO>(),
                Audios = new List<AudioUpdateDTO>(),
                Images = new List<ImageUpdateDTO>
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
                Videos = new List<VideoUpdateDTO>(),
                Partners = new List<PartnersUpdateDTO>(),
                Toponyms = new List<StreetcodeToponymCreateUpdateDTO>(),
                Subtitles = new List<SubtitleUpdateDTO>(),
                TimelineItems = new List<TimelineItemCreateUpdateDTO>(),
                RelatedFigures = new List<RelatedFigureUpdateDTO>(),
                Arts = new List<ArtCreateUpdateDTO>(),
                StreetcodeArtSlides = new List<StreetcodeArtSlideCreateUpdateDTO>(),
                StatisticRecords = new List<StatisticRecordUpdateDTO>(),
                StreetcodeCategoryContents = new List<StreetcodeCategoryContentUpdateDTO>(),
            };
        }
    }
}
