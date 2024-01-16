using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Tag
{
    public class TagClient: BaseClient
    {
        public TagClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetAllAsync(string authToken = "")
        {
            return await this.SendQuery($"/GetAll", authToken);
        }

        public async Task<RestResponse> GetByIdAsync(int id, string authToken = "")
        {
            return await this.SendQuery($"/GetById/{id}", authToken);
        }

        public async Task<RestResponse> GetByStreetcodeId(int id, string authToken = "")
        {
            return await this.SendQuery($"/getByStreetcodeId/{id}", authToken);
        }

        public async Task<RestResponse> GetTagByTitle(string title, string authToken = "")
        {
            return await this.SendQuery($"/GetTagByTitle/{title}", authToken);
        }
    }
}
