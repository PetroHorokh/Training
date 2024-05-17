using Microsoft.EntityFrameworkCore;
using Rent.DAL.Context;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;

namespace Rent.DAL.Repositories;

public class BillRepository(RentContext context) : RepositoryBase<Bill>(context), IBillRepository
{
}