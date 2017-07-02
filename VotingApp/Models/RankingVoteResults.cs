using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class RankingVoteResults
    {
        public RankingVoteResults()
        {
            Rankings = new Dictionary<int, int>();
        }
        public RankingVoteItem RankingVoteItem { get; set; }
        public Dictionary<int, int> Rankings { get; set; }
    }
}
