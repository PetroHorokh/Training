﻿namespace Rent.DAL.DTO;

public class TenantToCreateDto
{
    public string Name { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public Guid AddressId { get; set; }

    public string Director { get; set; } = null!;

    public string Description { get; set; } = null!;
}