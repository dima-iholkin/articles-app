using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;



namespace ArticlesApp.WebAPI.IdentityService.Services;



public class UserManagerOverride<TUser> : UserManager<TUser> where TUser : class
{
    public UserManagerOverride(
        IUserStore<TUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<TUser> passwordHasher,
        IEnumerable<IUserValidator<TUser>> userValidators,
        IEnumerable<IPasswordValidator<TUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<TUser>> logger
    ) : base(
        store,
        optionsAccessor,
        passwordHasher,
        userValidators,
        passwordValidators,
        keyNormalizer,
        errors,
        services,
        logger
    )
    { }



    public override async Task<IdentityResult> AddLoginAsync(
        TUser user, 
        UserLoginInfo login
    )
    {
        IdentityResult obj = await base.AddLoginAsync(user, login);

        IEnumerable<IdentityError> identityErrors = obj.Errors;
        if (identityErrors.Count() > 0)
        {
            IdentityError error = identityErrors.First();
            if (error.Code == "LoginAlreadyAssociated")
            {
                //error.
            }
        }

        return obj;
    }
}