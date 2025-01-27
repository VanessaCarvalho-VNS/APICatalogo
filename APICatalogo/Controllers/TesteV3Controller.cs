﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;


namespace APICatalogo.Controllers;

[Route("api/teste")]
[ApiController]
[ApiVersion(3)]
[ApiVersion(4)]
[ApiExplorerSettings(IgnoreApi = true)]
public class TesteV3Controller : ControllerBase
{
    [MapToApiVersion(3)]
    [HttpGet]

    public string GetVersion3()
    {
        return "Vesion3 - GET - Api versão 3.0";
    }

    [MapToApiVersion(4)]
    [HttpGet]

    public string GetVersion4()
    {
        return "Vesion4 - GET - Api versão 4.0";
    }
}
