namespace Streetcode.BLL.DTO.AdditionalContent.Coordinates.Update
{
    public class StreetcodeCoordinateUpdateDto
    {
        public int StreetcodeId { get; set; }
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longtitude { get; set; }
    }
}
