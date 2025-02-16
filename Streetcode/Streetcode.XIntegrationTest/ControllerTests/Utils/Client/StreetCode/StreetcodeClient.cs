using RestSharp;
using Streetcode.BLL.DTO.AdditionalContent.Filter;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateStatus;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode
{
    public class StreetcodeClient : StreetcodeRelatedBaseClient
    {
        public StreetcodeClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> UpdateAsync(StreetcodeUpdateDTO updateStreetcodeDto, string authToken = "")
        {
            return await SendCommand("/Update", Method.Put, updateStreetcodeDto, authToken);
        }

        public async Task<RestResponse> CreateAsync(StreetcodeCreateDTO createStreetcodeDto, string authToken = "")
        {
            return await SendCommand("/Create", Method.Post, createStreetcodeDto, authToken);
        }

        public async Task<RestResponse> GetAllAsync(GetAllStreetcodesRequestDTO request)
        {
            string queryString = QueryStringHelper<GetAllStreetcodesRequestDTO>.ToQueryString(request);
            return await SendQuery($"/GetAll{queryString}");
        }

        public async Task<RestResponse> GetAllPublishedAsync()
        {
            return await SendQuery("/GetAllPublished");
        }

        public async Task<RestResponse> GetAllShortAsync()
        {
            return await SendQuery("/GetAllShort");
        }

        public async Task<RestResponse> GetAllMainPageAsync()
        {
            return await SendQuery("/GetAllMainPage");
        }

        public async Task<RestResponse> GetPageMainPageAsync(ushort page, ushort pageSize)
        {
            return await SendQuery($"/GetPageMainPage?page={page}&pageSize={pageSize}");
        }

        public async Task<RestResponse> GetShortByIdAsync(int id)
        {
            return await SendQuery($"/GetShortById/{id}");
        }

        public async Task<RestResponse> GetByFilterAsync(StreetcodeFilterRequestDTO request)
        {
            string queryString = QueryStringHelper<StreetcodeFilterRequestDTO>.ToQueryString(request);
            return await SendQuery($"/GetByFilter{queryString}");
        }

        public async Task<RestResponse> ExistWithIndexAsync(int index)
        {
            return await SendQuery($"/ExistWithIndex/{index}");
        }

        public async Task<RestResponse> ExistWithUrlAsync(string url)
        {
            return await SendQuery($"/ExistWithUrl/{url}");
        }

        public async Task<RestResponse> GetAllCatalogAsync(int page, int count)
        {
            return await SendQuery($"/GetAllCatalog?page={page}&count={count}");
        }

        public async Task<RestResponse> GetCountAsync(bool? onlyPublished)
        {
            var query = onlyPublished.HasValue ? $"?onlyPublished={onlyPublished.Value}" : string.Empty;
            return await SendQuery($"/GetCount{query}");
        }

        public async Task<RestResponse> GetByTransliterationUrl(string url)
        {
            return await SendQuery($"/GetByTransliterationUrl/{url}");
        }

        public async Task<RestResponse> GetByQrIdAsync(int qrid)
        {
            return await SendQuery($"/GetByQrId/{qrid}");
        }

        public async Task<RestResponse> GetByIndexAsync(int index)
        {
            return await SendQuery($"/GetByIndex/{index}");
        }

        public async Task<RestResponse> PatchStageAsync(UpdateStatusStreetcodeByIdCommand request, string authToken = "")
        {
            return await SendCommand($"/PatchStage/{request.Id}/{request.Status}", Method.Put, request, authToken);
        }

        public async Task<RestResponse> SoftDeleteAsync(DeleteSoftStreetcodeCommand request, string authToken = "")
        {
            return await SendCommand<object>($"/SoftDelete/{request.Id}", Method.Delete, request, authToken);
        }

        public async Task<RestResponse> DeleteAsync(DeleteStreetcodeCommand request, string authToken = "")
        {
            return await SendCommand<object>($"/Delete/{request.Id}", Method.Delete, request, authToken);
        }
    }
}
