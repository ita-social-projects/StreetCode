using AutoMapper;
using Streetcode.BLL.DTO.Transactions;
using Streetcode.DAL.Entities.Transactions;

namespace Streetcode.BLL.Mapping.Transactions;

public class TransactionLinkProfile : Profile
{
    public TransactionLinkProfile()
    {
        CreateMap<TransactionLink, TransactLinkDTO>()
           .ReverseMap();
	}
}