using ArticlesApp.Core.Orchestrators.Exceptions;
using ArticlesApp.Core.Orchestrators.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace ArticlesApp.WebAPI.IdentityService;



[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRepository _userRepository;
    //private readonly ILogger<UserController> _logger;

    public UserController(
        IAuthenticationService authenticationService,
        IUserRepository userRepository
    //ILogger<UserController> logger
    )
    {
        _authenticationService = authenticationService;
        _userRepository = userRepository;
        //_logger = logger;
    }



    [AllowAnonymous]
    [HttpGet("_signed-in")]
    public async Task<IActionResult> GetIsSignedIn()
    {
        var result = await _authenticationService.AuthenticateAsync(
            HttpContext,
            "Identity.Application"
        );

        bool isSignedIn;
        if (result.Succeeded)
        {
            isSignedIn = true;
        }
        else
        {
            isSignedIn = false;
        }

        bool isSoftDeleted = false;
        if (isSignedIn)
        {
            Claim? userIdClaim = result.Principal!.Claims
                .Where(cl => cl.Type == "sub")
                .FirstOrDefault();

            if (userIdClaim == null)
            {
                isSoftDeleted = false;
            }

            try
            {
                isSoftDeleted = 
                    await _userRepository.IsSoftDeletedAsync(userIdClaim!.Value);
            }
            catch (EntityNotFoundException)
            {
                // no op.
            }
        }
        else
        {
            isSoftDeleted = false;
        }

        object responseBody = new
        {
            isSignedIn,
            isSoftDeleted
        };

        return Ok(responseBody);
    }
}
