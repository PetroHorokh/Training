﻿namespace Rent.Auth.WebAPI.ProblemDetails;

public class CustomProblemDetails : Microsoft.AspNetCore.Mvc.ProblemDetails
{
    public int? CustomStatusCode { get; set; }
}