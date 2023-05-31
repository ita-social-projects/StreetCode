using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update.Interfaces;

namespace Streetcode.BLL.DTO.Streetcode.Update
{
    public class PartnersUpdateDTO : IDeleted
    {
        bool IDeleted.IsDeleted { get; set; }
    }
}
