using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    public class StreetcodeControllerTests :
        BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public StreetcodeControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Streetcode")
        {

        }

        [Fact]
        [ExtractTestStreetcode]
        public async Task Update_ReturnSuccessStatusCode()
        {
            StreetcodeContent expectedStreetcode = ExtractTestStreetcode.StreetcodeForTest;

            var updateStreetcodeCommand = new UpdateStreetcodeCommand
            {
                Streetcode = new StreetcodeUpdateDTO
                {
                    Id = expectedStreetcode.Id,
                    Title = "New Title",
                    TransliterationUrl = "new-transliteration-url",
                }
            };
        }
    }
}
