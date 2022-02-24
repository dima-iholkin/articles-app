using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;



namespace ArticlesApp.WebAPI.Controllers;



public class OidcConfigurationController : Controller
{
    //private readonly ILogger<OidcConfigurationController> _logger;

    public IClientRequestParametersProvider ClientRequestParametersProvider { get; }

    public OidcConfigurationController(
        IClientRequestParametersProvider clientRequestParametersProvider
    //ILogger<OidcConfigurationController> logger
    )
    {
        ClientRequestParametersProvider = clientRequestParametersProvider;
        //_logger = logger;
    }



    [HttpGet("_configuration/{clientId}")]
    public IActionResult GetClientRequestParameters([FromRoute] string clientId)
    {
        IDictionary<string, string> clientRequestParameters =
            ClientRequestParametersProvider.GetClientParameters(
                HttpContext,
                clientId
            );

        return Ok(clientRequestParameters);
    }
}