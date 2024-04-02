using Microsoft.Data.SqlClient;

namespace Rent.DAL.Responses;

public class UpdatingResponse
{
    public DateTime DateTime { get; set; }

    public Exception? Error;
}