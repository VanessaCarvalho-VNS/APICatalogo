using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("api/v{version:apiVersion}/teste")]
[ApiController]
[ApiVersion("2.0")]
[ApiExplorerSettings(IgnoreApi = true)]
public class TesteV2Controller : ControllerBase
{
    [HttpGet]

    public string GetVersion()
    {
        return "TesteV2 - GET - Api Versão 2.0";
    }
}
