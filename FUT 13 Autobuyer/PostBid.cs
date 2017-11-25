using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Data;

namespace WindowsFormsApplication1
{
    public class PostBid
    {
        public void Check(string searchresponse, string resId, string maxPrice, int j)
        {
            int sk = Convert.ToInt32(maxPrice);
            List<string> resIds = new List<string>();
            List<string> tradeIds = new List<string>();
            List<int> currentBid = new List<int>();
            List<int> expires = new List<int>();
            List<int> startingBid = new List<int>();
            List<string> bidState = new List<string>();

            Player_AuctionInfo.RootObject returnedAuctions = new JavaScriptSerializer().Deserialize<Player_AuctionInfo.RootObject>(searchresponse);
            foreach (var item in returnedAuctions.auctionInfo)
            {
                resIds.Add(item.itemData.resourceId);
                tradeIds.Add(item.tradeId.ToString());
                currentBid.Add(item.currentBid);
                expires.Add(item.expires);
                startingBid.Add(item.startingBid);
                bidState.Add(item.bidState);
            }


            for (int i = 0; i < resIds.Count; i++)
            {
                if ((resIds[i] == resId) & (expires[i] < 300))
                {
                    if (currentBid[i] == 0)
                    {
                        Bid(tradeIds[i], startingBid[i],j);
                    }
                    else
                    {
                        if (bidState[i] == "highest")
                        {

                        }
                        else
                        {
                            if (sk > currentBid[i] && bidState[i] == "outbid")
                            {
                                ValueChecker v = new ValueChecker();
                                int bids = currentBid[i] + v.Value(currentBid[i]);
                                Bid(tradeIds[i], bids, j);
                            }
                        }
                    }
                }
            }
        }

        public void Bid(string tradeId, int currentBid, int i)
        {
            var post = "{\"bid\":" + currentBid + "}";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://utas.s2.fut.ea.com/ut/game/fifa13/trade/" + tradeId +"/bid");

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
            PlayerList.Number.Add(i.ToString());
            PlayerList.Id.Add(tradeId);
        }

        public void BidOnWatchList(string tradeId, int currentBid)
        {
            var post = "{\"bid\":" + currentBid + "}";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://utas.s2.fut.ea.com/ut/game/fifa13/trade/" + tradeId + "/bid");

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

        public DataTable CheckOverBid(DataTable input)
        {
            DataTable table = new DataTable();
            return table;
        }
    }
}
