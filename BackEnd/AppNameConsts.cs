namespace BackEnd
{
    public static class AppNameConsts
    {
        public static string GetAppName()
        {
            string appName = "backend";
#if !DEBUG
            appName = Environment.GetEnvironmentVariable("APP_NAME");
#endif
            return appName;
        }
    }
}
