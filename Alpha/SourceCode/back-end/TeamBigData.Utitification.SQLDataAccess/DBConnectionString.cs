namespace TeamBigData.Utification.SQLDataAccess;

public class DBConnectionString
{
    public string devSqlUsers { get; private set; }
    public string devSqlLogs { get; private set; }
    public string devSqlUserHash { get; private set; }
    public string devSqlUserProfile { get; set; }
    public string devSqlFeatures { get; private set; }
    
    public string devWinUsers { get; private set; }
    public string devWinLogs { get; private set; }
    public string devWinUserHash { get; private set; }
    public string devWinUserProfile { get; set; }
    public string devWinFeatures { get; private set; }

    public DBConnectionString()
    {
        // Sql Auth
        devSqlUsers = @"Server=localhost,1433;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=True;";
        devSqlLogs = @"Server=localhost,1433;Database=TeamBigData.Utification.Logs;Uid=AppUser;Pwd=t;TrustServerCertificate=True;Encrypt=True;";
        devSqlUserHash = @"Server=localhost,1433;Database=TeamBigData.Utification.UserHash;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=True;"; 
        devSqlUserProfile = @"Server=localhost,1433;Database=TeamBigData.Utification.UserProfile;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=True;";
        devSqlFeatures = @"Server=localhost,1433;Database=TeamBigData.Utification.Features;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=True;";
         
        // Win integrated Security
        devWinUsers = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
        devWinLogs = @"Server=.\;Database=TeamBigData.Utification.Logs;Uid=AppUser;Pwd=t;TrustServerCertificate=True;Encrypt=True;";
        devWinUserHash = @"Server=.\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False;"; 
        devWinUserProfile = @"Server=.\;Database=TeamBigData.Utification.UserProfile;Integrated Security=True;Encrypt=False;";
        devWinFeatures = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False;";
    }

}