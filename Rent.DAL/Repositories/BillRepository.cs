using Rent.DAL.Context;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;
using Rent.Model.Library;

namespace Rent.DAL.Repositories;

public class BillRepository(RentContext context) : RepositoryBase<Bill>(context), IBillRepository
{
}