namespace WindowsFormsApplication1
{
    public class CreateSearchlink
    {
        public string CreateBid(string name, string nation, string club,
            string position, string formation, string sk, string resId)
        {
            var searchstring = "";

            // Calculate Level
            var leveler = new DefineLevel();
            var levelname = leveler.Leveler(name);
            var level = leveler.LevelerName(levelname);
            searchstring += "&lev=" + level;

            // Calculate formation
            if (formation != "Any")
            {
                searchstring += "&form=f" + formation;
            }

            // Calculate position
            if ((position != "") & (position != "any"))
            {
                if ((position == "defense") | (position == "midfield") |
                    (position == "attacker"))
                {
                    searchstring += "&zone=" + position;
                }
                else
                {
                    searchstring += "&pos=" + position;
                }
            }

            // Calculate nation
            var z = nation.IndexOf('|');
            nation = nation.Substring(0, z);
            searchstring += "&nat=" + nation;

            // Calculate Team
            z = club.IndexOf('|');
            club = club.Substring(0, z);
            searchstring += "&team=" + club;

            // Calculate max bin price
            searchstring += "&macr=" + sk;

            return searchstring;
        }

        public string CreateBin(string name, string nation, string club,
            string position, string formation, string sk, string resId)
        {
            var searchstring = "";

            // Calculate level
            var leveler = new DefineLevel();
            var levelname = leveler.Leveler(name);
            var level = leveler.LevelerName(levelname);
            searchstring += "&lev=" + level;

            // Calculate formation
            if (formation != "Any")
            {
                searchstring += "&form=f" + formation;
            }

            // Calculate position
            if ((position != "") & (position != "any"))
            {
                if ((position == "defense") | (position == "midfield") |
                    (position == "attacker"))
                {
                    searchstring += "&zone=" + position;
                }
                else
                {
                    searchstring += "&pos=" + position;
                }
            }

            // Calculate Nation
            var z = nation.IndexOf('|');
            nation = nation.Substring(0, z);
            searchstring += "&nat=" + nation;

            // Calculate Team
            z = club.IndexOf('|');
            club = club.Substring(0, z);
            searchstring += "&team=" + club;

            // Calculate max bin price
            searchstring += "&maxb=" + (sk);

            return searchstring;
        }

        public string CreateCheck(string name, string nation, string club,
            string position, string formation, string sk, string resId)
        {
            var searchstring = "";

            //  Calculate level
            var leveler = new DefineLevel();
            var levelname = leveler.Leveler(name);
            var level = leveler.LevelerName(levelname);
            searchstring += "&lev=" + level;

            // Calculate formation
            if (formation != "Any")
            {
                searchstring += "&form=f" + formation;
            }

            // Calculate position
            if ((position != "") & (position != "any"))
            {
                if ((position == "defense") | (position == "midfield") |
                    (position == "attacker"))
                {
                    searchstring += "&zone=" + position;
                }
                else
                {
                    searchstring += "&pos=" + position;
                }
            }

            // Calculate nation
            var z = nation.IndexOf('|');
            nation = nation.Substring(0, z);
            searchstring += "&nat=" + nation;

            // Calculate team
            z = club.IndexOf('|');
            club = club.Substring(0, z);
            searchstring += "&team=" + club;

            return searchstring;
        }
    }
}
