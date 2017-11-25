using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private readonly string _filePath = ConfigurationManager.AppSettings["FilePath"];
        IList<RootObject> development;
        IList<RootObject> persons;
        Login db = new Login();
        Thread thread;
        int gewinn;
        

        public Form1()
        {
            InitializeComponent();
            LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": Programm gestartet");
            Control.CheckForIllegalCrossThreadCalls = false;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gewinn = 0;
            string json;
            using (StreamReader reader = new StreamReader(_filePath))
            {
                json = (reader.ReadLine().ToString());
            }

            persons = new JavaScriptSerializer().Deserialize<IList<RootObject>>(json);
            for (int i = 0; i < persons.Count; i++)
            {
                CB_player.Items.Add(persons[i].N.ToString());
            }
            CB_player.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            CB_player.AutoCompleteSource = AutoCompleteSource.ListItems;
            CB_player.Focus();


            json = "";
            using (StreamReader reader = new StreamReader(_filePath))
            {
                json = (reader.ReadLine().ToString());
            }
            development = new JavaScriptSerializer().Deserialize<IList<RootObject>>(json);
            for (int i = 0; i < development.Count; i++)
            {
                CB_development.Items.Add(development[i].N.ToString());
            }
            CB_development.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            CB_development.AutoCompleteSource = AutoCompleteSource.ListItems;
            CB_development.Focus();
        }

        private void BT_Import_Click(object sender, EventArgs e)
        {
            if (CB_player.Text == "")   {System.Windows.Forms.MessageBox.Show("Gib bitte einen Spieler an.");}
            else if (CB_nation.Text == "")   {System.Windows.Forms.MessageBox.Show("Gib bitte eine Nation an.");}
            else if (CB_club.Text == "")     {System.Windows.Forms.MessageBox.Show("Gib bitte einen Club an.");}
            else if (CB_position.Text == "") {System.Windows.Forms.MessageBox.Show("Gib bitte eine Position an.");}
            else if (CB_formation.Text == ""){System.Windows.Forms.MessageBox.Show("Gib bitte eine Formation an.");}
            else if (TB_SK.Text == "")       {System.Windows.Forms.MessageBox.Show("Gib bitte einen Kaufpreis an.");}
            else if (TB_SK2.Text == "") { System.Windows.Forms.MessageBox.Show("Gib bitte einen Verkaufspreis an."); }
            else if (cb_version.Text == "") { System.Windows.Forms.MessageBox.Show("Gib bitte eine Version an."); }
            else
            {
                long resID;
                if (cb_version.Text == "0")
                {
                    resID = persons[CB_player.SelectedIndex].Id;
                }
                else
                {
                    resID = persons[CB_player.SelectedIndex].Id + 50331648 + ((Convert.ToInt64(cb_version.Text) - 1) * 16777216);
                }
                
                string[] row = new string[] { CB_player.Text, CB_nation.Text, CB_club.Text, CB_position.Text, CB_formation.Text, TB_SK.Text, TB_SK2.Text, persons[CB_player.SelectedIndex].Id.ToString() };
                dataGridView1.Rows.Add(row);
            }
        }

        private void BT_clear_Click(object sender, EventArgs e)
        {
            CB_player.Text = "";
            CB_nation.Text = "";
            CB_club.Text = "";
            CB_position.Text = "";
            CB_formation.Text = "";
            TB_SK.Text = "";
            TB_SK2.Text = "";
        }

        private void BT_Login_Click(object sender, EventArgs e)
        {
            string email;
            string password;
            string securityhash;
            email = TB_email.Text;
            password = TB_password.Text;
            securityhash = TB_securityhash.Text;
            db.SendLogin(email, password, securityhash);
            LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": " + GuiHandler.LoginSuccess);
            
        }

        private void BT_list_import_Click(object sender, EventArgs e)
        {
            string Pfad = string.Empty;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                Pfad = openFileDialog1.FileName;
            if (Pfad == "")
            {
            }
            else
            {
                using (StreamReader sr = new StreamReader(Pfad))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        IList<string> list = line.Split('°');
                        string[] row = new string[] { list[0], list[1], list[2], list[3], list[4], list[5], list[6], list[7] };
                        dataGridView1.Rows.Add(row);
                    }
                }
            }
        }

        private void BT_list_export_Click(object sender, EventArgs e)
        {
            string content;
            int summe;
            summe = dataGridView1.Rows.Count - 1;
            content = "";
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = ".txt|*.txt";
            saveFileDialog1.Title = "Speichern der Liste";
            saveFileDialog1.ShowDialog();
            StreamWriter writer;
            writer = null;
            if (saveFileDialog1.FileName == "")
            {
            }
            else
            {
                writer = File.CreateText(saveFileDialog1.FileName);
            }

            dataGridView1.Rows.Count.ToString();
            for (int k = 0; k < dataGridView1.Rows.Count - 1; k++)
            {
                content = "";
                content += dataGridView1.Rows[k].Cells[0].Value.ToString() + "°";
                content += dataGridView1.Rows[k].Cells[1].Value.ToString() + "°";
                content += dataGridView1.Rows[k].Cells[2].Value.ToString() + "°";
                content += dataGridView1.Rows[k].Cells[3].Value.ToString() + "°";
                content += dataGridView1.Rows[k].Cells[4].Value.ToString() + "°";
                content += dataGridView1.Rows[k].Cells[5].Value.ToString() + "°";
                content += dataGridView1.Rows[k].Cells[6].Value.ToString() + "°";
                content += dataGridView1.Rows[k].Cells[7].Value.ToString();
                writer.WriteLine(content);
            }
            if (writer != null)
            {
                writer.Close();
            }
        }

        private void BT_start_buying_players_Click(object sender, EventArgs e)
        {
            TB_gewinn.Text = gewinn.ToString();
            thread = new Thread(delegate()
            {
                for (int aa = 0; aa < 999999; aa++)
                {
                    for (int zz = 0; zz < 15; zz++)
                    {
                        //#Pricecheck
                        string name, nation, club, position, formation, SK, ResID;
                        name = "";
                        nation = "";
                        club = "";
                        position = "";
                        formation = "";
                        SK = "";
                        ResID = "";
                        if ((dataGridView1.Rows.Count) - 1 == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Bitte geben sie zumindest einen Spieler in die Liste ein!");
                        }
                        else
                        {
                            //LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": Zum Pricecheck gekommen");
                            for (int i = 0; i < ((dataGridView1.Rows.Count) - 1); i++)
                            {
                                name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                nation = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                club = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                position = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                formation = dataGridView1.Rows[i].Cells[4].Value.ToString();
                                SK = dataGridView1.Rows[i].Cells[5].Value.ToString();
                                ResID = dataGridView1.Rows[i].Cells[7].Value.ToString();
                                CreateSearchlink searchlink = new CreateSearchlink();
                                string finalsearchstring = "https://utas.s2.fut.ea.com/ut/game/fifa13/auctionhouse?type=player&start=" + "0" + "&num=" + 230 + searchlink.CreateCheck(name, nation, club, position, formation, SK, ResID);
                                Search search = new Search();
                                string searchresponse = search.Start(finalsearchstring);
                                double temp;
                                double hilf;
                                double sellprice, buyprice;
                                ValueChecker v = new ValueChecker();
                                temp = v.AveragePrice(searchresponse, ResID);
                                hilf = v.RoundPrice(temp);
                                hilf = hilf * (Convert.ToDouble(tb_sell.Text) / 100);
                                sellprice = v.RoundPrice(hilf);
                                buyprice = v.RoundPrice(sellprice * (Convert.ToDouble(tb_buy.Text) / 100));


                                if (zz == 0 && aa == 0)
                                {
                                    dataGridView1.Rows[i].Cells[5].Value = buyprice;
                                    dataGridView1.Rows[i].Cells[6].Value = sellprice;
                                }
                                else
                                {
                                    if (Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value.ToString()) == 0 && sellprice != 0)
                                    {
                                        dataGridView1.Rows[i].Cells[5].Value = buyprice;
                                        dataGridView1.Rows[i].Cells[6].Value = sellprice;
                                    }
                                    if (sellprice < Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value.ToString()) && sellprice != 0)
                                    {
                                        dataGridView1.Rows[i].Cells[5].Value = buyprice;
                                        dataGridView1.Rows[i].Cells[6].Value = sellprice;
                                    }
                                }



                                //LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": Pricecheck ausgeführt für " + name + " (" + position + ", " + formation + ")");

                                if (i == 0)
                                {
                                    LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": Pricecheck ausgeführt für: " + dataGridView1.Rows[i].Cells[0].Value.ToString());
                                }
                                else
                                {
                                    LB_log.Items.RemoveAt(LB_log.Items.Count - 1);
                                    LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": Pricecheck ausgeführt für: " + dataGridView1.Rows[i].Cells[0].Value.ToString());
                                }
                                System.Threading.Thread.Sleep(2000);
                            }
                            LB_log.Items.RemoveAt(LB_log.Items.Count - 1);
                            LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": Pricecheck ausgeführt");
                        }


                        //# SUCHE

                        //string name, nation, club, position, formation, SK, ResID;
                        name = "";
                        nation = "";
                        club = "";
                        position = "";
                        formation = "";
                        SK = "";
                        ResID = "";

                        if ((dataGridView1.Rows.Count) - 1 == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Bitte geben sie zumindest einen Spieler an!");
                        }
                        else
                        {
                            for (int i = 0; i < ((dataGridView1.Rows.Count) - 1); i++)
                            {
                                name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                nation = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                club = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                position = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                formation = dataGridView1.Rows[i].Cells[4].Value.ToString();
                                SK = dataGridView1.Rows[i].Cells[5].Value.ToString();
                                ResID = dataGridView1.Rows[i].Cells[7].Value.ToString();
                                CreateSearchlink searchlink = new CreateSearchlink();
                                string finalsearchstring = "https://utas.s2.fut.ea.com/ut/game/fifa13/auctionhouse?type=player&start=" + "0" + "&num=" + "10" + searchlink.CreateBid(name, nation, club, position, formation, SK, ResID);
                                Search search = new Search();
                                string searchresponse = search.Start(finalsearchstring);
                                PostBid bid = new PostBid();
                                bid.Check(searchresponse, ResID, SK, i);
                            }
                        }
                        for (int i = 0; i < PlayerList.Id.Count(); i++)
                        {
                            listBox1.Items.Add(dataGridView1.Rows[Convert.ToInt16(PlayerList.Number[i])].Cells[0].Value.ToString() + " :|: " + PlayerList.Id[i] + " :|: " + PlayerList.Number[i]);
                        }




                        listBox2.Items.Clear();
                        GetPileCards Watchlist = new GetPileCards();
                        DataTable table = new DataTable();
                        table = Watchlist.WatchList();
                        if (table.Rows.Count == 0)
                        {
                            System.Threading.Thread.Sleep(250000);
                        }
                        while (table.Rows.Count != 0)
                        {
                            for (int k = 0; k < table.Rows.Count; k++)
                            {
                                int currentBid = Convert.ToInt32(table.Rows[k].ItemArray[0].ToString());
                                int sec = Convert.ToInt32(table.Rows[k].ItemArray[1].ToString());
                                string tradeState = table.Rows[k].ItemArray[2].ToString();
                                string bidState = table.Rows[k].ItemArray[3].ToString();
                                string tradeID = table.Rows[k].ItemArray[4].ToString();
                                string ID = table.Rows[k].ItemArray[5].ToString();


                                int number; // Nummer in der Liste (Die mit den Preisen)
                                number = -1;

                                //Checken der TradeID in der PlayerList
                                for (int j = 0; j < PlayerList.Number.Count; j++)
                                {
                                    if (tradeID == PlayerList.Id[j])
                                    {
                                        number = Convert.ToInt32(PlayerList.Number[j]);
                                    }
                                }

                                //System.Windows.Forms.MessageBox.Show(number.ToString());
                                if (number != -1)
                                {
                                    listBox2.Items.Add(
                                        dataGridView1.Rows[number].Cells[0].Value.ToString() + " - " +
                                        sec.ToString() + " Sekunden - " +
                                        currentBid.ToString() + " - " +
                                        tradeState + " - " +
                                        bidState + " -  " +
                                        tradeID
                                    );
                                }

                                if (number != -1 && sec < 12 && currentBid < Convert.ToInt32(dataGridView1.Rows[number].Cells[5].Value.ToString()) && tradeState == "active" && bidState != "highest")
                                {
                                    ValueChecker v = new ValueChecker();
                                    PostBid p = new PostBid();
                                    p.BidOnWatchList(tradeID, currentBid + v.Value(currentBid));
                                }

                                if (number != -1 && currentBid >= Convert.ToInt32(dataGridView1.Rows[number].Cells[5].Value.ToString()) && bidState != "highest")
                                {
                                    Watchlist.RemoveItemfromWatchList(tradeID);
                                }
                                if (sec == -1 && bidState != "highest")
                                {
                                    Watchlist.RemoveItemfromWatchList(tradeID);

                                }

                                if (number != -1 && bidState == "highest" && tradeState == "closed")
                                {
                                    LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": " + dataGridView1.Rows[number].Cells[0].Value.ToString() + " erboten für " + currentBid);
                                    try
                                    {
                                        Watchlist.MovetoTp(tradeID, ID);
                                        Watchlist.SellOnTp(ID, Convert.ToInt32(dataGridView1.Rows[number].Cells[6].Value.ToString()));
                                    }
                                    catch (WebException ex)
                                    {
                                        LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": " + ex.ToString());
                                    }
                                    LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": " + dataGridView1.Rows[number].Cells[0].Value.ToString() + " wird verkauft für " + dataGridView1.Rows[number].Cells[6].Value.ToString() +
                                        " (Profit: " + (Convert.ToInt32(dataGridView1.Rows[number].Cells[6].Value.ToString()) * 95 / 100 - Convert.ToInt32(currentBid)).ToString() + ")");
                                    gewinn += (Convert.ToInt32(dataGridView1.Rows[number].Cells[6].Value.ToString()) * 95 / 100 - Convert.ToInt32(currentBid));
                                    PlayerList.Profit.Add(Convert.ToInt32(dataGridView1.Rows[number].Cells[6].Value.ToString()) * 95 / 100 - Convert.ToInt32(currentBid));
                                    TB_avgProfit.Text = PlayerList.Profit.Average().ToString();
                                    TB_gewinn.Text = gewinn.ToString();
                                }
                            }
                            System.Threading.Thread.Sleep(2000);
                            table = Watchlist.WatchList();
                            System.Threading.Thread.Sleep(2000);
                            try
                            {
                                Watchlist.RemoveExpiredFromTp();
                            }
                            catch (WebException ex)
                            {
                                LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": " + ex);
                            }
                            listBox2.Items.Clear();
                        }
                        PlayerList.Number.Clear();
                        PlayerList.Id.Clear();
                        //LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": " + zz.ToString() + ". Durchlauf beendet");
                    }
                    string email;
                    string password;
                    string securityhash;
                    email = TB_email.Text;
                    password = TB_password.Text;
                    securityhash = TB_securityhash.Text;
                    db.SendLogin(email, password, securityhash);
                    LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": " + GuiHandler.LoginSuccess);

                    //#Erneuter Pricecheck
                    LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": Korrekturpricecheck wird ausgeführt");
                    for (int i = 0; i < ((dataGridView1.Rows.Count) - 1); i++)
                    {
                        string name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        string nation = dataGridView1.Rows[i].Cells[1].Value.ToString();
                        string club = dataGridView1.Rows[i].Cells[2].Value.ToString();
                        string position = dataGridView1.Rows[i].Cells[3].Value.ToString();
                        string formation = dataGridView1.Rows[i].Cells[4].Value.ToString();
                        string SK = dataGridView1.Rows[i].Cells[5].Value.ToString();
                        string ResID = dataGridView1.Rows[i].Cells[7].Value.ToString();
                        CreateSearchlink searchlink = new CreateSearchlink();
                        string finalsearchstring = "https://utas.s2.fut.ea.com/ut/game/fifa13/auctionhouse?type=player&start=" + "0" + "&num=" + 230 + searchlink.CreateCheck(name, nation, club, position, formation, SK, ResID);
                        Search search = new Search();
                        string searchresponse = search.Start(finalsearchstring);
                        double temp;
                        double hilf;
                        double sellprice, buyprice;
                        ValueChecker v = new ValueChecker();
                        temp = v.AveragePrice(searchresponse, ResID);
                        hilf = v.RoundPrice(temp);
                        hilf = hilf * (Convert.ToDouble(tb_sell.Text) / 100);
                        sellprice = v.RoundPrice(hilf);
                        buyprice = v.RoundPrice(sellprice * (Convert.ToDouble(tb_buy.Text) / 100));

                        if ((sellprice * 150 / 100) < Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value))
                        {
                            dataGridView1.Rows[i].Cells[5].Value = buyprice;
                            dataGridView1.Rows[i].Cells[6].Value = sellprice;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[6].Value = sellprice * 110 / 100;
                            dataGridView1.Rows[i].Cells[5].Value = buyprice * 110 / 100;
                        }
                    }
                    LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": Korrekturpricecheck wurde ausgeführt");
                }
            });
            thread.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetPileCards PileCards = new GetPileCards();
            DataTable table = new DataTable();
            table = PileCards.Tradepile();
            dataGridView2.DataSource = table;
            dataGridView2.Refresh();


            string json;
            using (StreamReader reader = new StreamReader(_filePath))
            {
                json = (reader.ReadLine().ToString());
            }

            persons = new JavaScriptSerializer().Deserialize<IList<RootObject>>(json);
            for (int v = 0; v < dataGridView2.Rows.Count-1; v++)
            {
                for (int i = 0; i < persons.Count; i++)
                {
                    if (persons[i].Id.ToString() == dataGridView2.Rows[v].Cells[0].Value.ToString())
                    {
                        dataGridView2.Rows[v].Cells[0].Value = persons[i].N.ToString();
                    }
                }
            }

        }

        public string FirstName
        {
            get { return ""; }
            set { LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": " + value); }

        }

        public void SetData(string SID)
        {
            //LoginData data = new LoginData();
            //data.SID = SID;
            //textBox2.Text = data.SID;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*Thread thread = new Thread(delegate()
            {*/
                listBox2.Items.Clear();
                GetPileCards Watchlist = new GetPileCards();
                DataTable table = new DataTable();
                table = Watchlist.WatchList();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    int currentBid = Convert.ToInt32(table.Rows[i].ItemArray[0].ToString());
                    int sec = Convert.ToInt32(table.Rows[i].ItemArray[1].ToString());
                    string tradeState = table.Rows[i].ItemArray[2].ToString();
                    string bidState = table.Rows[i].ItemArray[3].ToString();
                    string tradeID = table.Rows[i].ItemArray[4].ToString();
                    string ID = table.Rows[i].ItemArray[5].ToString();


                    int number; // Nummer in der Liste (Die mit den Preisen)
                    number = -1;

                    //Checken der TradeID in der PlayerList
                    for (int j = 0; j < PlayerList.Number.Count; j++)
                    {
                        if (tradeID == PlayerList.Id[j])
                        {
                            number = Convert.ToInt32(PlayerList.Number[j]);
                        }
                    }

                    listBox2.Items.Add(
                        dataGridView1.Rows[number].Cells[0].Value.ToString() + " - " +
                        sec.ToString() + " Sekunden - " +
                        currentBid.ToString() + " - " +
                        tradeState + " - " +
                        bidState + " -  " +
                        tradeID
                    );

                    if (number != -1 && sec < 12 && currentBid < Convert.ToInt32(dataGridView1.Rows[number].Cells[5].Value.ToString()) && tradeState == "active" && bidState != "highest")
                    {
                        ValueChecker v = new ValueChecker();
                        PostBid p = new PostBid();
                        p.BidOnWatchList(tradeID, currentBid + v.Value(currentBid));
                    }

                    if (number != -1 && currentBid >= Convert.ToInt32(dataGridView1.Rows[number].Cells[5].Value.ToString()) && bidState != "highest")
                    {
                        Watchlist.RemoveItemfromWatchList(tradeID);
                    }
                    if (sec == -1 && bidState != "highest")
                    {
                        Watchlist.RemoveItemfromWatchList(tradeID);
                    }

                    if (number != -1 && bidState == "highest" && tradeState == "closed")
                    {
                        Watchlist.MovetoTp(tradeID, ID);
                        Watchlist.SellOnTp(ID, Convert.ToInt32(dataGridView1.Rows[number].Cells[6].Value.ToString()));
                    }

                }
                System.Threading.Thread.Sleep(4500);
            
        }

        private void BT_Pricecheck_Click(object sender, EventArgs e)
        {
            string name, nation, club, position, formation, SK, ResID;
            name = "";
            nation = "";
            club = "";
            position = "";
            formation = "";
            SK = "";
            ResID = "";
            if ((dataGridView1.Rows.Count) - 1 == 0)
            {
                System.Windows.Forms.MessageBox.Show("Bitte geben sie zumindest einen Spieler in die Liste ein!");
            }
            else
            {
                for (int i = 0; i < ((dataGridView1.Rows.Count) - 1); i++)
                {
                    name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    nation = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    club = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    position = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    formation = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    SK = dataGridView1.Rows[i].Cells[5].Value.ToString();
                    ResID = dataGridView1.Rows[i].Cells[7].Value.ToString();
                    CreateSearchlink searchlink = new CreateSearchlink();
                    string finalsearchstring = "https://utas.s2.fut.ea.com/ut/game/fifa13/auctionhouse?type=player&start=" + "0" + "&num=" + 230 + searchlink.CreateCheck(name, nation, club, position, formation, SK, ResID);
                    Search search = new Search();
                    string searchresponse = search.Start(finalsearchstring);
                    double temp;
                    double hilf;
                    double sellprice, buyprice;
                    ValueChecker v = new ValueChecker();
                    temp = v.AveragePrice(searchresponse, ResID);
                    hilf=v.RoundPrice(temp);
                    hilf = hilf * (Convert.ToDouble(tb_sell.Text) / 100);
                    sellprice = v.RoundPrice(hilf);
                    buyprice = v.RoundPrice(sellprice * (Convert.ToDouble(tb_buy.Text) / 100));
                    dataGridView1.Rows[i].Cells[5].Value = buyprice;
                    dataGridView1.Rows[i].Cells[6].Value = sellprice;
                    System.Threading.Thread.Sleep(2000);
                    LB_log.Items.Add(System.DateTime.Now.ToLongTimeString() + ": Pricecheck ausgeführt für " + name + " (" + position +", "+ formation + ")");
                }
            }
        }

        private void BT_stop_buying_consumables_Click(object sender, EventArgs e)
        {

        }

        private void BT_stop_buying_players_Click(object sender, EventArgs e)
        {
            thread.Abort();
        }

    }

    public class RootObject
    {
        public int Id { get; set; }
        public string N { get; set; }
        public string C { get; set; }
    }
}
