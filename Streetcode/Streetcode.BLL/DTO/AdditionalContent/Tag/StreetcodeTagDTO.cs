namespace Streetcode.BLL.DTO.AdditionalContent.Tag
{
    public class StreetcodeTagDTO : CreateUpdateTagDTO
    {
        public int Id { get; set; }
        public bool IsVisible { get; set; }
        public int Index { get; set; }
    }
}
