namespace FrontEnd
{
    public static class AppNameConsts
    {
        public static string GetAppName()
        {
            string appName = "frontend1";
#if !DEBUG
            appName = Environment.GetEnvironmentVariable("APP_NAME");
#endif
            return appName;
        }


        public static string GetBackendAppName()
        {
            string appName = GetAppName();
            string backendAppName = $"backend{appName.Replace("frontend","")}";
            return backendAppName;
        }
    }
}
