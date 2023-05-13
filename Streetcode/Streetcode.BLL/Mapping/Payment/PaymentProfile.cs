using AutoMapper;
using Streetcode.BLL.DTO.Payment;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Entities.Payment;
using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.BLL.Mapping.Payment;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<InvoiceInfo, PaymentResponseDTO>().ReverseMap();
    }
}