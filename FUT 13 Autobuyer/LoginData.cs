using System.Net;

namespace WindowsFormsApplication1
{
    public class LoginData
    {
        private static LoginData _instance;
        public static string Sid;
        public static CookieContainer CookC;

        private LoginData() { }
        public static LoginData Instance => _instance ?? (_instance = new LoginData());
    }
}
