namespace ArticlesApp.Core.Entities.PersonalData;



public class PersonalData
{
    public PersonalData(
        IEnumerable<PersonalData_Article> articles,
        PersonalData_UserInfo userInfo
    )
    {
        Articles = articles;
        UserInfo = userInfo;
    }

    public IEnumerable<PersonalData_Article> Articles { get; }

    public PersonalData_UserInfo UserInfo { get; }
}