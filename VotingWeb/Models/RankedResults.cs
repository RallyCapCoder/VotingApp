using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotingWeb.Models
{
    public class RankedResults
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Party { get; set; }
        public int? Ranking { get; set; }
        public bool HasWon { get; set; }
    }
}