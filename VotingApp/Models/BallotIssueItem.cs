using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class BallotIssueItem
    {
        public Guid VoteIssueId { get; set; }
        public string Name { get; set; }
        public string OfficalName { get; set; }
        public string Description { get; set; }
        public string Subtext { get; set; }
        public bool VotedFor { get; set; }
    }

}
