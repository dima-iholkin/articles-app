namespace ArticlesApp.Core.Entities.PersonalData;



public class PersonalData_UserInfo
{
    public PersonalData_UserInfo(
        string id,
        string userName,
        string email
    )
    {
        Id = id;
        UserName = userName;
        Email = email;
    }

    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }
}