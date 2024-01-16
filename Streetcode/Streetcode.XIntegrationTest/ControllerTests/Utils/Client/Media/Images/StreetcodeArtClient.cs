using RestSharp;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images
{
    public class StreetcodeArtClient : BaseClient
    {
        public StreetcodeArtClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetByStreetcodeId(int id, string authToken = "")
        {
            return await this.SendQuery($"/GetByStreetcodeId/{id}", authToken);
        }
    }
}
