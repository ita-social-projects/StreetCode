using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Transactions
{
    public class TransactionLinkUpdateDto : TransactLinkDto, IModelState
    {
        public ModelState ModelState { get; set; }
    }
}
