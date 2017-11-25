using System.Collections.Generic;

namespace WindowsFormsApplication1
{
    public class UserClubList
    {
        public string year { get; set; }
        public string platform { get; set; }
        public int established { get; set; }
        public int lastAccessTime { get; set; }
        public string clubName { get; set; }
        public string clubAbbr { get; set; }
    }

    public class Persona
    {
        public int personaId { get; set; }
        public string personaName { get; set; }
        public List<UserClubList> userClubList { get; set; }
    }

    public class UserAccountInfo
    {
        public List<Persona> personas { get; set; }
    }

    public class AccInfo2
    {
        public UserAccountInfo userAccountInfo { get; set; }
    }
}
