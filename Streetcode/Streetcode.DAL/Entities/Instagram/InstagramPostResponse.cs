namespace Streetcode.DAL.Entities.Instagram
{
    public class InstagramPostResponse
    {
        public IEnumerable<InstagramPost> Data { get; set; } = Enumerable.Empty<InstagramPost>();
    }
}
