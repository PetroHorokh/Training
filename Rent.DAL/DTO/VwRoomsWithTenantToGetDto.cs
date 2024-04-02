namespace Rent.DAL.DTO;

public class VwRoomsWithTenantToGetDto
{
    public int Number { get; set; }

    public string? Name { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public override string ToString() =>
        Name != null
            ? $"Room: {Number}\nTenant name: {Name}\nDates: {StartDate} - {EndDate}"
            : $"There was no tenant in this room";
}