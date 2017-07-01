using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.Models
{
    public class VoteResults
    {
        public Guid VoteResultsId { get; set; }
        public Guid BallotId { get; set; }
        public Guid? CanidateId { get; set; }
        public Guid? VoteIssueId { get; set; }
        public bool VotedFor { get; set; }
        public int Ranking { get; set; }
    }
}
