namespace Streetcode.BLL.DTO.Jobs
{
    public class JobDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool Status { get; set; }
        public string Description { get; set; } = null!;
        public string Salary { get; set; } = null!;
    }
}