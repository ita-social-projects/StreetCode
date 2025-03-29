using System.Reflection;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.DAL.Entities.Media;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media;

public class ExtractTestAudioAttribute : BeforeAfterTestAttribute
{
    public static AudioFileBaseCreateDTO AudioCreateDtoForTest { get; set; } = null!;

    public static AudioFileBaseUpdateDTO AudioUpdateDtoForTest { get; set; } = null!;

    public static AudioDTO AudioDtoForTest { get; set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        AudioCreateDtoForTest = new AudioFileBaseCreateDTO
        {
            Title = "TestAudio",
            BaseFormat = "fakebase64string",
            MimeType = "audio/mpeg",
            Extension = "mp3",
        };
        AudioUpdateDtoForTest = new AudioFileBaseUpdateDTO
        {
            Title = "UpdatedTestAudio",
            BaseFormat = "fakeBase64String",
            MimeType = "audio/mpeg",
            Extension = "mp3",
        };
        AudioDtoForTest = new AudioDTO
        {
            BlobName = "testblobname",
            Base64 = "testbase64string",
            MimeType = "audio/mpeg",
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var audio = sqlDbHelper.GetExistItem<Audio>(a => a.Id == AudioDtoForTest.Id);
        if (audio != null)
        {
            sqlDbHelper.DeleteItem(audio);
            sqlDbHelper.SaveChanges();
        }
    }
}