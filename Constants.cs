namespace prof_tester_api
{
    public static class Constants
    {
        //Менять этот url в формате: "http://localhost:8080"
        public static readonly string serverUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS").Split(";").First();

        public static readonly string localPathToStorages = @"Resources/";
        public static readonly string localPathToProfileIcons = $"{localPathToStorages}ProfileIcons/";
        public static readonly string localPathToLecternFile = $"{localPathToStorages}Lecterns/";

        public static readonly string webPathToProfileIcons = $"{serverUrl}/api/upload/profileIcon/";
        public static readonly string webPathToLecternFile = $"{serverUrl}/api/upload/lectern/";
    }
}