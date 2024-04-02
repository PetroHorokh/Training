using Microsoft.EntityFrameworkCore;
using Rent.DAL.DTO;
using Rent.DAL.Models;
using Rent.DAL.Repositories.Contracts;
using Rent.DAL.RepositoryBase;

namespace Rent.DAL.Repositories;

public class ImpostRepository(RentContext context) : RepositoryBase<Impost>(context), IImpostRepository
{
    public async Task CreateWithProcedure(ImpostToCreateDto impost)
    {
        await Context.Database.ExecuteSqlAsync($"EXEC [dbo].[sp_Impost_Insert] @Tax = {impost.Tax}, @Fine = {impost.Tax}, @PaymentDay = {impost.PaymentDay}, @StartDay = '{impost.StartDate}', @EndDay = '{impost.EndDate}'");
    }
}