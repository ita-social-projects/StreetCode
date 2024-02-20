namespace Streetcode.BLL.DTO.ReCaptchaResponseDTO
{
    public class ReCaptchaResponseDto
    {
        public bool Success { get; set; }
        public string[] ErrorCodes { get; set; } = null!;
    }
}
