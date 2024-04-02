using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.BLL.Profiles;

public class PaymentProfile : MappingProfile
{
    public PaymentProfile()
    {
        CreateMap<Bill, BillToGetDto>();
    }
}