using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingWeb.Models
{
    public class MultipleVotesResult
    {
        public string Name { get; set; }
        public string Party { get; set; }
        public int Votes { get; set; }

    }
}