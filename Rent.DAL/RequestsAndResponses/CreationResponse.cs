﻿namespace Rent.DAL.RequestsAndResponses;

public class CreationResponse
{
    public Guid? CreatedId { get; set; }

    public Exception? Error { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.Now;
}