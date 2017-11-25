using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace WindowsFormsApplication1
{
    public class ValueChecker
    {
        public int Value(int input)
        {
            int betrag = 0;
            if (input == 0)
            {
                betrag = 150;
            }
            else if (input < 1000)
            {
                betrag = 50;
            }
            else if (input < 10000)
            {
                betrag = 100;
            }
            else if (input < 50000)
            {
                betrag = 250;
            }
            else if (input < 100000)
            {
                betrag = 500;
            }
            else if (input >= 100000)
            {
                betrag = 1000;
            }
            return betrag;
        }

        public double AveragePrice(string search, string resId)
        {
            var check = true;
            List<int> buyNowPrice = new List<int>();
            List<int> avg = new List<int>();
            Player_AuctionInfo.RootObject returnedAuctions = new JavaScriptSerializer().Deserialize<Player_AuctionInfo.RootObject>(search);
            foreach (var item in returnedAuctions.auctionInfo)
            {
                if (resId == item.itemData.resourceId)
                {
                    buyNowPrice.Add(item.buyNowPrice);
                }
            }
            double average = 0;

            if (buyNowPrice.Count != 0)
            {
                for (int v = 0; v < 3; v++)
                {
                    //System.Windows.Forms.MessageBox.Show(buyNowPrice.Count().ToString());
                    var lowestPrice = 15000000;
                    var hilf = 0;
                    for (int i = 0; i < buyNowPrice.Count; i++)
                    {
                        if (buyNowPrice[i] <= lowestPrice && buyNowPrice[i] != 0)
                        {
                            lowestPrice = buyNowPrice[i];
                            hilf = i;
                        }
                    }
                    //System.Windows.Forms.MessageBox.Show(lowestPrice.ToString() + "; " + hilf);
                    avg.Add(lowestPrice);
                    if (hilf != 0)
                    {
                        buyNowPrice.RemoveAt(hilf);
                    }
                    else
                    {
                        check = false;
                    }
                }
                average = check ? avg.Average() : 0;
            }
            return average;
        }

        public int RoundPrice(double input)
        {
            int temp;
            if (input <= 1000)
            {
                temp = (int)Math.Round(input / 50.0) * 50;
            }
            else if (input <= 10000)
            {
                temp = (int)Math.Round(input / 100.0) * 100;
            }
            else if (input <= 50000)
            {
                temp = (int)Math.Round(input / 250.0) * 250;
            }
            else if (input <= 50000)
            {
                temp = (int)Math.Round(input / 500.0) * 500;
            }
            else
            {
                temp = (int)Math.Round(input / 1000.0) * 1000;
            }

            return temp;
        }

        public int ValueSell(int input)
        {
            int betrag = 0;
            if (input <= 1000)
            {
                betrag = 50;
            }
            else if (input <= 10000)
            {
                betrag = 100;
            }
            else if (input <= 50000)
            {
                betrag = 250;
            }
            else if (input <= 100000)
            {
                betrag = 500;
            }
            else if (input >= 100000)
            {
                betrag = 1000;
            }
            return betrag;
        }
    }
}
