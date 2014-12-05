using System.Collections.Generic;

namespace ClashCaller.Models
{
    public class War
    {
        public string ClanName { get; set; }
        public string ClanOpponent { get; set; }

        public IList<Attack> Attacks { get; set; }

        public int TotalRanks { get; set; }
    }
}
