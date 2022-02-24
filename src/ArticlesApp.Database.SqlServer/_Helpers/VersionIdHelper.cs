using ArticlesApp.Database.SqlServer.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;



namespace ArticlesApp.Database.SqlServer._Helpers;



public static class VersionIdHelper
{
    public static PropertyEntry<Article_SqlServer, short>  AddProperVersionIdCheck(
        this PropertyEntry<Article_SqlServer, short> versionIdEntry, 
        short originalVersionId
    )
    {
        versionIdEntry.OriginalValue = originalVersionId;
        return versionIdEntry;
    }



    public static void IncrementVersionId(this PropertyEntry<Article_SqlServer, short> versionIdEntry)
    {
        versionIdEntry.CurrentValue += 1;
    }
}