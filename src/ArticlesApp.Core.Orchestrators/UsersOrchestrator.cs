using ArticlesApp.Core.Entities.Notification;
using ArticlesApp.Core.Entities.PersonalData;
using ArticlesApp.Core.Orchestrators.Infrastructure;
using ArticlesApp.Core.Orchestrators.Models.Articles;
using ArticlesApp.Database.Models;
using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;



namespace ArticlesApp.Core.Orchestrators;



public class UsersOrchestrator
{
    private readonly IArticlesRepository _articlesRepository;
    private readonly NotificationsOrchestrator _notificationsOrchestrator;
    private readonly UserManager<ApplicationUser_DB> _userManager;
    private readonly IUserRepository _usersRepository;
    //private readonly ILogger<UsersOrchestrator> _logger;

    public UsersOrchestrator(
        IArticlesRepository articlesRepository,
        IUserRepository usersRepository,
        UserManager<ApplicationUser_DB> userManager,
        NotificationsOrchestrator notificationsOrchestrator
    //ILogger<UsersOrchestrator> logger
    )
    {
        _articlesRepository = articlesRepository;
        _usersRepository = usersRepository;
        _userManager = userManager;
        _notificationsOrchestrator = notificationsOrchestrator;
        //_logger = logger;
    }



    public async Task<IdentityResult> CreateUserAsync(ApplicationUser_DB user)
    {
        IdentityResult result = await _userManager.CreateAsync(user);

        NotificationToCreate_Submitted notif = new NotificationToCreate_Submitted()
        {
            Message = "Thanks for registering. Now explore the app!",
            Reciever_UserId = user.Id,
            NotificationType_TypeId = NotificationTypesEnum.UserRegistered
        };
        await _notificationsOrchestrator.TryCreateNotificationAsync(notif);

        return result;
    }



    public async Task<IdentityResult> CreateUserAsync(
        ApplicationUser_DB user,
        string? password
    )
    {
        IdentityResult result = await _userManager.CreateAsync(
            user,
            password
        );

        NotificationToCreate_Submitted notif = new NotificationToCreate_Submitted()
        {
            Message = "Thanks for registering. Now explore the app!",
            Reciever_UserId = user.Id,
            NotificationType_TypeId = NotificationTypesEnum.UserRegistered
        };
        await _notificationsOrchestrator.TryCreateNotificationAsync(notif);

        return result;
    }



    public async Task<PersonalData> UserDownloadsPersonalDataAsync(string userId)
    {
        IEnumerable<PersonalData_Article> articles = await _articlesRepository.UserDownloadsPersonalDataAsync(userId);
        PersonalData_UserInfo userInfo = await _usersRepository.UserDownloadsPersonalDataAsync(userId);

        return new PersonalData(
            articles,
            userInfo
        );
    }
}