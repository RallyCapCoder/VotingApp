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
        public VoteIssue Issue { get; set; }
        public Canidate Canidate { get; set; }
        public bool YesVote { get; set; }
        public bool NoVote { get; set; }
    }
}
