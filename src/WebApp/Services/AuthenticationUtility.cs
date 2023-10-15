namespace WebApp.Services
{
    public static class AuthenticationUtility
    {
        public static string? IsAuthenticated(HttpContext? httpContext)
        {
            if (httpContext != null)
            {
                string? expireTimeAsString = httpContext.Session.GetString("ExpireTime");
                DateTime expireTime;

                if (!string.IsNullOrEmpty(expireTimeAsString) && DateTime.TryParse(expireTimeAsString, out expireTime) && DateTime.Now < expireTime)
                {
                    return httpContext.Session.GetString("UserName");
                }
            }
            return null;
        }
    }
}
