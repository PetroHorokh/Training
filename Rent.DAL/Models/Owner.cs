﻿using System.Text.Json.Serialization;
using temp;

namespace Rent.DAL.Models;

public class Owner
{
    public Guid OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid AddressId { get; set; }

    [JsonIgnore]
    public Address Address { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Asset> Assets { get; set; } = new List<Asset>();
}
