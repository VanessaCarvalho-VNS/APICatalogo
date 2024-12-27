using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("api/v{version:apiVersion}/teste")]
[ApiController]
[ApiVersion("1.0")]
[ApiExplorerSettings(IgnoreApi = true)]
public class TesteV1Controller : ControllerBase
{
    [HttpGet]
    public string GetVersion()
    {
        return "TesteV1 - GET - Api Versão 1.0";
    }
}
