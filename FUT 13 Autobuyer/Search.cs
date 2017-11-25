using System;
using System.Text;
using System.Net;
using System.IO;

namespace WindowsFormsApplication1
{
    public class Search
    {
        public string Start(string finalsearchstring)
        {
            const string post = "";
            HttpWebRequest req =
                (HttpWebRequest) WebRequest.Create(finalsearchstring);

            req.CookieContainer = LoginData.CookC;
            req.ContentType = "application/json";
            req.Method = "POST";
            req.Headers.Add("X-UT-SID", LoginData.Sid);
            req.Headers.Add("x-http-method-override", "GET");
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] loginDataBytes = encoding.GetBytes(post);
            req.ContentLength = loginDataBytes.Length;
            //System.Windows.Forms.MessageBox.Show(loginDataBytes.Length.ToString());
            Stream stream = req.GetRequestStream();
            stream.Write(loginDataBytes, 0, loginDataBytes.Length);
            stream.Close();


            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            var wichtig = str.ReadToEnd();
            stream.Close();
            return wichtig;
        }
    }
}
