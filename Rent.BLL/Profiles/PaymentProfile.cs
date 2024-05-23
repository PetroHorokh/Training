using Rent.DTOs.Library;
using Rent.Model.Library;

namespace Rent.BLL.Profiles;

public class PaymentProfile : MappingProfile
{
    public PaymentProfile()
    {
        CreateMap<Bill, BillToGetDto>();
    }
}