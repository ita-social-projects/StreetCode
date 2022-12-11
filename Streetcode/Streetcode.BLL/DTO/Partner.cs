
using Models;
namespace DTO
{
    public class Partner 
    {

        public int Id;

        public Image Image;

        public string Title;

        public string Description;

        public HashSet<Url> Urls;

    }
}