namespace User.Api.Constants
{
    public class EnvironmentConstant
    {
        public const string JWTConfiguration = "jwt";

        public const string AllowAllOriginPolicy = "_allowAllOriginPolicy";

        public const string UserDbConnectionString = "ConnectionStrings:UserDbConnection";

        public const string EnableDatabaseDetailedErrors = "Database:EnableDetailedErrors";

        public const string EnableDatabaseSensitiveLogging = "Database:EnableSensitiveDataLogging";       

        public const string GoogleAuthenticationClientId = "ExternalAuthentication:Google:ClientId";

        public const string GoogleAuthenticationClientSecret = "ExternalAuthentication:Google:ClientSecret";
    }
}
