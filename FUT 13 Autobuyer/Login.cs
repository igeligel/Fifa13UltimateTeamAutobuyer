using System;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace WindowsFormsApplication1
{
    public class Login
    {
        public string SessionId;

        //-----------------Zeit bestimmen-----------------\\
        public static int php_Time()
        {
            return (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        //-----------------Login senden-----------------\\
        public void SendLogin(string email, string password, string securityhash)
        {
            GuiHandler.LoginSuccess = "Login nicht erfolgreich";
            Login login = new Login();
            const string url = "https://www.ea.com/uk/football/services/authenticate/login";
            var post = "email=" + email + "&password=" + password + "&stay-signed=ON";

            CookieContainer cookieContainer = new CookieContainer();
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = cookieContainer;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
            req.MediaType = "HTTP/1.1";


            req.Referer = "http://www.ea.com/uk/football/login?redirectUrl=http://www.ea.com/uk/football/fifa-ultimate-team";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] loginDataBytes = encoding.GetBytes(post);
            req.ContentLength = loginDataBytes.Length;
            Stream stream = req.GetRequestStream();
            stream.Write(loginDataBytes, 0, loginDataBytes.Length);
            stream.Close();
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            var wichtig = str.ReadToEnd();
            stream.Close();
            foreach (Cookie cookie in res.Cookies)
            {
                cookieContainer.Add(cookie);
            }

            var nucId = wichtig.Split('>');

            string success = nucId[2].Replace("</success", "");
            if (success == "1")
            {
                var nucid = nucId[7].Replace("</nucleusId", "");
                login.GetSharedInfo(cookieContainer, nucid, securityhash);
            }
            else
            {
                
            }
        }

        //-----------------GetSharedInfo-----------------\\
        public void GetSharedInfo(CookieContainer cookieContainer, string nucid, string securityhash)
        {
            int time = php_Time();
            string url = "http://www.ea.com/p/fut/a/card/l/en_GB/s/p/ut/shards?timestamp=" + time;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = cookieContainer;
            req.ContentType = "application/json;";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
            req.Referer = "http://www.ea.com/uk/football/fifa-ultimate-team";
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            str.ReadToEnd();
            dataStream.Close();
            AccInfo1(cookieContainer, nucid, securityhash);
        }

        //-----------------AccInfo1-----------------\\
        public void AccInfo1(CookieContainer cookieContainer, string nucid, string securityhash)
        {
            int time = php_Time();
            string url = "http://www.ea.com/p/fut/a/card-360/l/en_GB/s/p/ut/game/fifa13/user/accountinfo?timestamp=" + time;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = cookieContainer;
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
            req.Referer = "http://www.ea.com/uk/football/fifa-ultimate-team";
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            str.ReadToEnd();
            dataStream.Close();
            AccInfo2(cookieContainer, nucid, securityhash);
        }

        //-----------------AccInfo2-----------------\\
        public void AccInfo2(CookieContainer cookieContainer, string nucid, string securityhash)
        {
            var personaName = "";
            var personaId = "";
            int time = php_Time();
            string url = "http://www.ea.com/p/fut/a/card-pc/l/en_GB/s/p/ut/game/fifa13/user/accountinfo?timestamp=" + time;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = cookieContainer;
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
            req.Referer = "http://www.ea.com/uk/football/fifa-ultimate-team";
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            var wichtig = str.ReadToEnd();
            dataStream.Close();
            AccInfo2 returnedResponse = new JavaScriptSerializer().Deserialize<AccInfo2>(wichtig);
            foreach (var item in returnedResponse.userAccountInfo.personas)
            {
                personaName = item.personaName;
                personaId = item.personaId.ToString();
            }
            Auth(cookieContainer, nucid, personaName, personaId, securityhash);
        }

        //-----------------Authentification-----------------\\
        public void Auth(CookieContainer cookieContainer, string nucid, string personaName, string personaId, string securityhash)
        {
            string URL = "http://www.ea.com/p/fut/a/card-ps3/l/en_GB/s/p/ut/auth";
            string authData = "{ \"isReadOnly\": false, \"sku\": \"393A0001\", \"clientVersion\": 3, \"nuc\": " + nucid + ", \"nucleusPersonaId\": " + personaId + ", \"nucleusPersonaDisplayName\": \"" + personaName + "\", \"nucleusPersonaPlatform\": \"" + "ps3" + "\", \"locale\": \"en-GB\", \"method\": \"idm\", \"priorityLevel\":4, \"identification\": { \"EASW-Token\": \"\" } }";
            string post = authData;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            req.CookieContainer = cookieContainer;
            req.Method = "POST";
            req.ContentType = "application/json; charset=UTF-8;";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
            req.MediaType = "HTTP/1.1";


            req.Referer = "http://www.ea.com/uk/football/fifa-ultimate-team";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] loginDataBytes = encoding.GetBytes(post);
            req.ContentLength = loginDataBytes.Length;
            Stream stream = req.GetRequestStream();
            stream.Write(loginDataBytes, 0, loginDataBytes.Length);
            stream.Close();
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            var wichtig = str.ReadToEnd();
            stream.Close();
            //System.Windows.Forms.MessageBox.Show(wichtig);
            var sid = "";
            int z = 145;
            while (z < 181)
            {
                sid = sid + wichtig[z];
                z = z + 1;
            }
            Question(cookieContainer,sid, securityhash);
        }

        //-----------------Security Hash-----------------\\
        public void Question(CookieContainer cookieContainer,string sid, string securityhash)
        {
            int time = php_Time();
            string url = "http://www.ea.com/p/fut/a/card-ps3/l/en_GB/s/p/ut/game/fifa13/phishing/question?timestamp=" + time;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = cookieContainer;
            req.Headers.Add("X-UT-SID", sid);
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
            req.Referer = "http://www.ea.com/uk/football/fifa-ultimate-team";
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            str.ReadToEnd();
            dataStream.Close();

            Validate(cookieContainer,sid, securityhash);
        }
        
        public void Validate(CookieContainer cookieContainer,string sid, string securityhash)
        {
            string URL = "http://www.ea.com/p/fut/a/card-ps3/l/en_GB/s/p/ut/game/fifa13/phishing/validate";
            string post = "answer=" + securityhash;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
            req.CookieContainer = cookieContainer;
            req.Method = "POST";
            req.Headers.Add("X-UT-SID", sid);
            req.Headers.Add("X-Ut-Embed-Error", "true");
            req.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";


            req.Referer = "http://www.ea.com/uk/football/fifa-ultimate-team";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] loginDataBytes = encoding.GetBytes(post);
            req.ContentLength = loginDataBytes.Length;
            Stream stream = req.GetRequestStream();
            stream.Write(loginDataBytes, 0, loginDataBytes.Length);
            stream.Close();
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            str.ReadToEnd();
            stream.Close();
            foreach (Cookie cookie in res.Cookies)
            {
                cookieContainer.Add(cookie);
            }
            SessionId = sid;
            LoginData.Sid = SessionId;
            LoginData.CookC = cookieContainer;
            GuiHandler.LoginSuccess = "Login war erfolgreich";
        }
    }
}
