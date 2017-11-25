using System;

namespace WindowsFormsApplication1
{
    public class DefineLevel
    {
        public string Leveler(string name)
        {
            var words = name.Split('(');
            var level = words[1].Split(')');
            return level[0];
        }

        public string LevelerName(string levelnumber)
        {
            var l = Convert.ToInt32(levelnumber);
            if (l <= 64)
            {
                return "bronze";
            }
            if (l >= 65 & l <= 74)
            {
                return "silver";
            }
            return l >= 75 ? "gold" : "";
        }
    }
}
