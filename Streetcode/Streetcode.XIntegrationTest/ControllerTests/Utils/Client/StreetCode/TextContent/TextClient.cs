using RestSharp;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode.TextContent;

public class TextClient : StreetcodeRelatedBaseClient
{
    public TextClient(HttpClient client, string secondPathUrl = "")
        : base(client, secondPathUrl)
    {
    }

    public async Task<RestResponse> UpdateParsedText(TextPreviewDTO textPreviewDto, string authToken = "")
    {
        return await SendCommand("/UpdateParsedText", Method.Post, textPreviewDto, authToken);
    }
}