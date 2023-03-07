namespace Streetcode.WebApi.Requests.Streetcode;

public record GetAllStreetcodesRequest(
        int Page = 1,
        int Amount = 10,
        string? Title = null,
        string? Sort = null,
        string? Filter = null);
