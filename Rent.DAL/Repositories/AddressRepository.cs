using Microsoft.EntityFrameworkCore;
using Rent.DAL.Context;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;

namespace Rent.DAL.Repositories;

public class AddressRepository(RentContext context) : RepositoryBase<Address>(context), IAddressRepository
{
    public async Task CreateWithProcedure(AddressToCreateDto address)
    {
        await Context.Database.ExecuteSqlAsync($"EXEC [dbo].[sp_Address_Insert] @City = '{address.City}', @Street = '{address.Street}', @Building = '{address.Building}', @CreatedBy = '{address.CreatedBy}'");
    }
}