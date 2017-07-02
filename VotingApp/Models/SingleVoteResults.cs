using System.Collections.Generic;

namespace VotingApp.Models
{
    public class SingleVoteResults
    {
        public SingleVoteResults()
        {
            Votes = new Dictionary<bool, int>();
        }
        public SingleVoteItem SingleVoteItem { get; set; }
        public Dictionary<bool, int> Votes { get; set; }
    }
}