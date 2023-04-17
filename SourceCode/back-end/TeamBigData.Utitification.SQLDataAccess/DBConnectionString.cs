namespace TeamBigData.Utification.SQLDataAccess;

public class DBConnectionString
{
    public string _connectionStringUsers { get; private set; }
    public string _connectionStringLogs { get; private set; }
    public string _connectionStringUserHash { get; private set; }
    public string _connectionStringUserProfile { get; set; }
    public string _connectionStringFeatures { get; private set; }

    public DBConnectionString()
    {
        _connectionStringUsers = @"Server=localhost,1433;Database=TeamBigData.Utification.Users;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=True;";
        _connectionStringLogs = @"Server=localhost,1433;Database=TeamBigData.Utification.Logs;Uid=AppUser;Pwd=t;TrustServerCertificate=True;Encrypt=True;";
        _connectionStringUserHash = @"Server=localhost,1433;Database=TeamBigData.Utification.UserHash;Uid=root;Pwd=root;TrustServerCertificate=True;Encrypt=True;"; 
        _connectionStringUserProfile = @"Server=localhost,1433;Database=TeamBigData.Utification.UserProfile;Uid=root;Pwd=root;TrustServerCertificate=True;Encryption=False;";
        _connectionStringFeatures = @"Server=localhost,1433;Database=TeamBigData.Utification.Features;Uid=root;Pwd=root;TrustServerCertificate=True;Encryption=False;";
         
    }

}