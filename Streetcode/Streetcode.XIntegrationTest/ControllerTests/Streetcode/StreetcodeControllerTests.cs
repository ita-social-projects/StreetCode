using Streetcode.XIntegrationTest.ControllerTests.Utils;
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
            :base(factory, "/api/Streetcode")
        {

        }



    }
}
