namespace Streetcode.BLL.Middleware
{
    public class RequestResponseMiddlewareOptions
    {
        public int MaxResponseLength { get; set; }
        public List<string> PropertiesToIgnore { get; set; } = new List<string>();
    }
}
