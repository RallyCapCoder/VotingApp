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
        public Guid? RankingVoteId { get; set; }
        public RankingVoteItem RankingVoteItem { get; set; }
        public int? Ranking { get; set; }
        public Guid? MultipleVoteId { get; set; }
        public MultipleVoteItem MultipleVoteItem { get; set; }
        public bool? VotedFor { get; set; }
        public Guid? SingleVoteId { get; set; }
        public SingleVoteItem SingleVoteItem { get; set; }
        public bool? VotedYes { get; set; }
        public bool? VotedNo { get; set; }
    }
}
