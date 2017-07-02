using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Models;

namespace VotingApp.DataManagement.Builders
{
    public class RankingVoteTicketBuilder
    {
        public CanidateBuilder CanidateBuilder { get; set; }

        public RankingVoteTicketBuilder()
        {
            CanidateBuilder = new CanidateBuilder();
        }

        public RankingVoteItem GetModel(RankingVote rankingVote)
        {
            return new RankingVoteItem()
            {
                RankingVoteItemId = rankingVote.RankingVoteId,
                PrimeCanidate = CanidateBuilder.GetModel(rankingVote.PrimeCanindate),
                SubCanidate = CanidateBuilder.GetModel(rankingVote.SubCanindate),
            };
        }
    }
}
