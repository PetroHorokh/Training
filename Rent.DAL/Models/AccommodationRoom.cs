﻿using System.Text.Json.Serialization;

namespace Rent.DAL.Models;

public class AccommodationRoom
{
    public Guid AccommodationRoomId { get; set; }

    public int AccommodationId { get; set; }

    public Guid RoomId { get; set; }

    public int Quantity { get; set; }

    [JsonIgnore]
    public Accommodation Accommodation { get; set; } = null!;

    [JsonIgnore]
    public Room Room { get; set; } = null!;
}
