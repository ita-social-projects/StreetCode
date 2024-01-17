﻿using RestSharp;
using RestSharp.Serializers;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.Update;
namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class StreetcodeClient
    {
        protected RestClient Client;

        public string SecondPartUrl { get; }

        public StreetcodeClient(HttpClient client, string secondPartUrl = "")
        {
            this.Client = new RestClient(client) { AcceptedContentTypes = ContentType.JsonAccept };
            this.SecondPartUrl = secondPartUrl;
        }

        public async Task<RestResponse> GetAllAsync()
        {
            return await this.GetResponse($"/GetAll");
        }

        public async Task<RestResponse> GetByIdAsync(int id)
        {
            return await this.GetResponse($"/GetById/{id}");
        }

        public async Task<RestResponse> GetByStreetcodeId(int id)
        {
            return await this.GetResponse($"/getByStreetcodeId/{id}");
        }

        public async Task<RestResponse> GetArtsByStreetcodeId(int id)
        {
            return await this.GetResponse($"/getArtsByStreetcodeId/{id}");
        }

        public async Task<RestResponse> GetResponse(string requestString)
        {
            var request = new RestRequest($"{this.SecondPartUrl}{requestString}");
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddHeader("content-type", "application/json");
            var returns = await this.Client.ExecuteGetAsync(request);
            return returns;
        }

        public async Task<RestResponse> UpdateAsync(StreetcodeUpdateDTO updateStreetcodeDTO)
        {
            var request = new RestRequest($"{this.SecondPartUrl}/Update", Method.Put);
            request.AddJsonBody(updateStreetcodeDTO);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddHeader("content-type", "application/json");
            var response = await this.Client.ExecuteAsync(request);
            return response;
        }

        public async Task<RestResponse> CreateAsync(StreetcodeCreateDTO createStreetcodeDTO)
        {
            var request = new RestRequest($"{this.SecondPartUrl}/Create", Method.Post);
            request.AddJsonBody(createStreetcodeDTO);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            request.AddHeader("content-type", "application/json");
            var response = await this.Client.ExecuteAsync(request);
            return response;
        }
    }
}
