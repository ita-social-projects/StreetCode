using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Transactions
{
    public class TransactionLinkUpdateDTO : TransactLinkDTO, IModelState
    {
        public ModelState ModelState { get; set; }
    }
}
