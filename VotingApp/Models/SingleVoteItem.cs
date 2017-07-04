using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class SingleVoteItem
    {
        public Guid SingleVoteTicketId { get; set; }
        public BallotIssueItem Issue { get; set; }
        public CandidateItem CandidateItem { get; set; }
        public bool YesVote { get; set; }
        public bool NoVote { get; set; }
    }
}
