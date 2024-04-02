namespace Rent.DAL.DTO;

public class VwCertificateForTenantToGetDto
{
    public Guid? RentId { get; set; }

    public DateTime? RentStartDate { get; set; }

    public DateTime? RentEndDate { get; set; }

    public string? BillIds { get; set; }

    public string? PaymentIds { get; set; }

    public override string ToString() =>
        RentStartDate != null
            ? $"Certificate for rent with id {RentId}\nDates of rent: {RentStartDate} - {RentStartDate}\nInformation concerning billing:\n{BillIds ?? "Empty"}\nInformation concerning payment:\n{PaymentIds ?? "Empty"}\n"
            : $"Certificate is empty";
}