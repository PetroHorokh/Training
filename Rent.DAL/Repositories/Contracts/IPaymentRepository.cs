using Rent.DAL.RepositoryBase;
using Rent.DTOs.Library;
using Rent.Model.Library;
using Rent.ResponseAndRequestLibrary;

namespace Rent.DAL.Repositories.Contracts;

public interface IPaymentRepository : IRepositoryBase<Payment>
{
    Task<Response<Guid>> CreateWithProcedure(PaymentToCreateDto payment);
}