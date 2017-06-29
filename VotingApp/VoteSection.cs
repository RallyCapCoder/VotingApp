using System;
using System.Collections.Generic;
using System.Text;

namespace VotingApp
{
    public abstract class VoteSection
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<VoteItem> Items { get; set; }
    }
}
