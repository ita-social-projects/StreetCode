namespace Streetcode.BLL.DTO.Feedback;

public class ResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Email { get; set; } = null!;
    public string? Description { get; set; }
}