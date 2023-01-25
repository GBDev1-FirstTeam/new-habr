namespace NewHabr.Domain;

public static class Constants
{
    public const string EmpyString = "";
    public const string ConfigurationJSON = "configuration.json";
    public const string AppSettingsJSON = "appsettings.json";
    public const string True = "True";
    public const string False = "False";

    public static class SQLProvider
    {
        public const string ConfigurationName = "SQLServerProvider";
        public const string MSSQL = "mssql";
        public const string PostgreSQL = "postgresql";
    }

    public static class Authorization
    {
        public const string UserId = "UserId";
        public const string UserName = "UserName";
        public const string Roles = "Roles";
    }

    public static class Roles
    {
        public const string Administrator = "Administrator";
        public const string Moderator = "Moderator";
        public const string User = "User";
        public const string NotRegistered = "NotRegistered";
    }

    public static class RolesRus
    {
        public const string Administrator = "Администратор";
        public const string Moderator = "Модератор";
        public const string User = "Пользователь";
        public const string NotRegistered = "Незарегистрированный пользователь";
    }

    public static class Settings
    {
        public const string WebUISettings = "WebUISettings";
        public const string WebApiSettings = "WebApiSettings";
    }

    public static class Identity
    {
        public const string System = "System";
        public const string ErrMsgHasNotAccess = "У Вас нет прав на выполнение этой операции.";
    }

    public static class ControllerMethods
    {
        public const string Create = "create";
        public const string Load = "load/{id}";
        public const string List = "list";
        public const string Update = "update";
        public const string Delete = "delete/{id}";
    }
}
