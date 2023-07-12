namespace Streetcode.BLL.Middleware
{
    public class MiddlewareOptions
    {
        public int MaxResponseLength { get; set; }
        public List<string> PropertiesToShorten { get; set; }
        public List<string> PropertiesToIgnore { get; set; }
    }
}
