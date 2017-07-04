using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VotingApp.Models;

namespace VotingWeb.Models
{
    public class VotingViewModel 
    {
        public Guid BallotId { get; set; }
        public List<RankingVoteItem> PresidentAndVicePres { get; set; }
        public RankingVoteItem PresidentWriteIn { get; set; }
        public SingleVoteItem SupremeCourt { get; set; }
        public List<MultipleVoteItem> StateRep { get; set; }
        public MultipleVoteItem StateRepWriteIn { get; set; }
        public SingleVoteItem BallotIssue { get; set; }
    }
}