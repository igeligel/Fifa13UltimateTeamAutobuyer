using System.Collections.Generic;

namespace WindowsFormsApplication1
{
    public class PlayerList
    {
        private static PlayerList _instance;
        public static List<string> Number = new List<string>();
        public static List<string> Id = new List<string>();
        public static List<int> Profit = new List<int>();

        private PlayerList() { }
        public static PlayerList Instance => _instance ?? (_instance = new PlayerList());
    }
}
