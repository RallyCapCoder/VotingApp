using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingWeb.Models
{
    public class SingleVoteResults
    {
        public string Name { get; set; }
        public int YesVotes { get; set; }
        public int NoVotes { get; set; }
    }
}