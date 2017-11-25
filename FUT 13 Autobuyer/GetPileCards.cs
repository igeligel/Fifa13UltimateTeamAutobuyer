using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Data;

namespace WindowsFormsApplication1
{
    public class GetPileCards
    {
        public DataTable Tradepile()
        {
            //-----------------Anlegen der Listen-----------------\\
            var cardIdList = new List<string>();
            var buyNowPrice = new List<string>();
            var startingBid = new List<string>();
            var timeLeft = new List<string>();
            var formation = new List<string>();
            var tradeState = new List<string>();


            const string post = " ";
            var req = (HttpWebRequest) WebRequest.Create(
                "https://utas.s2.fut.ea.com/ut/game/fifa13/tradepile");

            req.CookieContainer = LoginData.CookC;
            req.ContentType = "application/json";
            req.Method = "POST";
            req.Headers.Add("X-UT-SID", LoginData.Sid);
            req.Headers.Add("x-http-method-override", "GET");
            req.UserAgent =
                "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

            var encoding = new ASCIIEncoding();
            var loginDataBytes = encoding.GetBytes(post);
            req.ContentLength = loginDataBytes.Length;
            var stream = req.GetRequestStream();
            stream.Write(loginDataBytes, 0, loginDataBytes.Length);
            stream.Close();


            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            var wichtig = str.ReadToEnd();
            stream.Close();

            
            JSS_Tradepile.RootObject returnedAuctions = new JavaScriptSerializer().Deserialize<JSS_Tradepile.RootObject>(wichtig);
            foreach (var item in returnedAuctions.auctionInfo)
            {
                cardIdList.Add(item.itemData.resourceId);
                buyNowPrice.Add(item.buyNowPrice.ToString());
                startingBid.Add(item.startingBid.ToString());
                timeLeft.Add(item.expires.ToString());
                formation.Add(item.itemData.formation);
                tradeState.Add(item.tradeState ?? "null");
            }

            DataTable table = new DataTable();
            table.Columns.Add("cardID", typeof(string));
            table.Columns.Add("buyNowPrice", typeof(string));
            table.Columns.Add("startingBid", typeof(string));
            table.Columns.Add("timeLeft", typeof(string));
            table.Columns.Add("formation", typeof(string));
            table.Columns.Add("bidState", typeof(string));
            for (int i = 0; i < cardIdList.Count; i++)
            {
                int x = Convert.ToInt32(timeLeft[i]);
                TimeSpan span = new TimeSpan(0, 0, x);
                int normalizedHours = span.Hours;
                int normalizedMinutes = span.Minutes;
                int normalizedSeconds = span.Seconds;

                table.Rows.Add(cardIdList[i], buyNowPrice[i], startingBid[i],
                    (normalizedHours.ToString() + ":" +
                     normalizedMinutes.ToString() + ":" +
                     normalizedSeconds.ToString()), formation[i],
                    tradeState[i]);
                {
                }
            }
            return table;
        }

        public DataTable WatchList()
        {
            List<int> currentBid = new List<int>();
            List<int> timeLeft = new List<int>();
            List<string> tradeState = new List<string>();
            List<string> bidState = new List<string>();
            List<string> tradeIdList = new List<string>();
            List<string> id = new List<string>();


            DataTable table = new DataTable();

            string POST = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://utas.s2.fut.ea.com/ut/game/fifa13/watchlist");

            req.CookieContainer = LoginData.CookC;
            req.ContentType = "application/json";
            req.Method = "POST";
            req.Headers.Add("X-UT-SID", LoginData.Sid);
            req.Headers.Add("x-http-method-override", "GET");
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] loginDataBytes = encoding.GetBytes(POST);
            req.ContentLength = loginDataBytes.Length;
            Stream stream = req.GetRequestStream();
            stream.Write(loginDataBytes, 0, loginDataBytes.Length);
            stream.Close();


            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream dataStream = res.GetResponseStream();
            StreamReader str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            var wichtig = str.ReadToEnd();
            stream.Close();

            JSS_Watchlist.RootObject returnedAuctions = new JavaScriptSerializer().Deserialize<JSS_Watchlist.RootObject>(wichtig);

            foreach (var item in returnedAuctions.auctionInfo)
            {
                currentBid.Add(item.currentBid);
                timeLeft.Add(item.expires);
                tradeState.Add(item.tradeState);
                bidState.Add(item.bidState);
                tradeIdList.Add(item.tradeId.ToString());
                id.Add(item.itemData.id.ToString());
            }
            table.Columns.Add("currentBid", typeof(int));
            table.Columns.Add("timeLeft", typeof(int));
            table.Columns.Add("tradeState", typeof(string));
            table.Columns.Add("bidState", typeof(string));
            table.Columns.Add("tradeID", typeof(string));
            table.Columns.Add("ID", typeof(string));
            for (int i = 0; i < tradeIdList.Count; i++)
            {
                table.Rows.Add(currentBid[i], timeLeft[i], tradeState[i], bidState[i], tradeIdList[i] , id[i]);
            }
            //System.Windows.Forms.MessageBox.Show(
            //    "currentBid: " + table.Rows[0].ItemArray[0].ToString() + Environment.NewLine +
            //    "timeLeft: " + table.Rows[0].ItemArray[1].ToString() + Environment.NewLine +
            //    "tradeState: " + table.Rows[0].ItemArray[2].ToString() + Environment.NewLine +
            //    "bidState: " + table.Rows[0].ItemArray[3].ToString() + Environment.NewLine +
            //    "tradeID: " + table.Rows[0].ItemArray[4].ToString()
            //    );
            return table;
        }

        public class Person
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        public void RemoveItemfromWatchList(string tradeId)
        {
            string POST = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://utas.s2.fut.ea.com/ut/game/fifa13/watchlist?tradeId=" + tradeId);

            req.CookieContainer = LoginData.CookC;
            req.ContentType = "application/json";
            req.Method = "POST";
            req.Headers.Add("X-UT-SID", LoginData.Sid);
            req.Headers.Add("x-http-method-override", "DELETE");
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] loginDataBytes = encoding.GetBytes(POST);
            req.ContentLength = loginDataBytes.Length;
            Stream stream = req.GetRequestStream();
            stream.Write(loginDataBytes, 0, loginDataBytes.Length);
            stream.Close();


            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream dataStream = res.GetResponseStream();
            var unused = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
            stream.Close();
        }

        public void MovetoTp(string tradeId, string id)
        {
            string post = "{\"itemData\":[{\"pile\":\"trade\",\"tradeId\":" + tradeId + ",\"id\":\"" + id + "\"}]}" ;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://utas.s2.fut.ea.com/ut/game/fifa13/item");

            req.CookieContainer = LoginData.CookC;
            req.ContentType = "application/json";
            req.Method = "POST";
            req.Headers.Add("X-UT-SID", LoginData.Sid);
            req.Headers.Add("x-http-method-override", "PUT");
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

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
        }

        public void SellOnTp(string id, int price)
        {
            var v = new ValueChecker();
            var hilfprice = price - v.ValueSell(price);
            //System.Windows.Forms.MessageBox.Show(hilfprice + "; " + Price);
            string post = "{\"itemData\":{\"id\":" + id + "},\"buyNowPrice\":" + price + ",\"duration\":43200,\"startingBid\":" + hilfprice + "}";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://utas.s2.fut.ea.com/ut/game/fifa13/auctionhouse");

            req.CookieContainer = LoginData.CookC;
            req.ContentType = "application/json";
            req.Method = "POST";
            req.Headers.Add("X-UT-SID", LoginData.Sid);
            req.Headers.Add("x-http-method-override", "POST");
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:20.0) Gecko/20100101 Firefox/20.0";

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

            //System.Windows.Forms.MessageBox.Show(wichtig);
        }

        public void RemoveExpiredFromTp()
        {
            string url = "https://utas.s2.fut.ea.com/ut/game/fifa13/tradepile";
            string post = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.CookieContainer = LoginData.CookC;
            req.ContentType = "application/json";
            req.Method = "POST";
            req.Headers.Add("X-UT-SID", LoginData.Sid);
            req.Headers.Add("x-http-method-override", "GET");
            req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";

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
            


            List<string> expires = new List<string>();
            List<string> tradeIDs = new List<string>();
            List<string> tradeState = new List<string>();

            JSS_Tradepile.RootObject returnedResponse = new JavaScriptSerializer().Deserialize<JSS_Tradepile.RootObject>(wichtig);
            foreach (var item in returnedResponse.auctionInfo)
            {
                expires.Add(item.expires.ToString());
                tradeIDs.Add(item.tradeId);
                //resID.Add(item.itemData.resourceId.ToString());
                //Bid.Add(item.currentBid.ToString());
                tradeState.Add(item.tradeState);
            }
            for (int i = 0; i < expires.Count; i++) // Loop through List with for
            {
                if (expires[i] == "-1" && tradeState[i] == "closed")
                {
                    url = "https://utas.s2.fut.ea.com/ut/game/fifa13/trade/" + tradeIDs[i];
                    post = "";
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.CookieContainer = LoginData.CookC;
                    req.ContentType = "application/json";
                    req.Method = "POST";
                    req.Headers.Add("X-UT-SID", LoginData.Sid);
                    req.Headers.Add("x-http-method-override", "DELETE");
                    req.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";

                    encoding = new ASCIIEncoding();
                    loginDataBytes = encoding.GetBytes(post);
                    req.ContentLength = loginDataBytes.Length;
                    stream = req.GetRequestStream();
                    stream.Write(loginDataBytes, 0, loginDataBytes.Length);
                    stream.Close();
                    res = (HttpWebResponse)req.GetResponse();

                    dataStream = res.GetResponseStream();
                    str = new StreamReader(dataStream ?? throw new InvalidOperationException(), Encoding.UTF8);
                    str.ReadToEnd();
                    stream.Close();
                }
            }
        }
    }
}
