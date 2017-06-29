using System;
using System.Collections.Generic;
using System.Text;

namespace VotingApp
{
    public class Jurisdiction
    {
        public string Title { get; set; }
        public List<VoteSection> Sections { get; set; }
    }
}
