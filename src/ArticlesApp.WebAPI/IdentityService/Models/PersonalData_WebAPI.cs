using ArticlesApp.Core.Entities.PersonalData;



namespace ArticlesApp.WebAPI.IdentityService.Models;



public class PersonalData_WebAPI
{
    public PersonalData_WebAPI(
        IEnumerable<PersonalData_Article> articles,
        Dictionary<string, string> userInfo
    )
    {
        Articles = articles;
        UserInfo = userInfo;
    }

    public IEnumerable<PersonalData_Article> Articles { get; }

    public Dictionary<string, string> UserInfo { get; }
}