using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingApp.Models;

namespace VotingWeb.Models
{
    public class VotingViewModel 
    {
        public Guid BallotId { get; set; }
        public CanindateParty DemocraticCanidates { get; set; }
        public CanindateParty RepublicanCanidates { get; set; }
        public CanindateParty IndependentCanidates { get; set; }
        public CanindateParty SupremeCourt { get; set; }
        public VoteIssue BallotIssue { get; set; }
    }
}