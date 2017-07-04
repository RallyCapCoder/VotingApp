using VotingApp.Context;
using VotingApp.Models;

namespace VotingApp.Builders
{
    public class RankingVoteTicketBuilder
    {
        public CandidateBuilder CandidateBuilder { get; set; }

        public RankingVoteTicketBuilder()
        {
            CandidateBuilder = new CandidateBuilder();
        }

        public RankingVoteItem GetModel(RankingVote rankingVote)
        {
            return new RankingVoteItem()
            {
                RankingVoteItemId = rankingVote.RankingVoteId,
                PrimeCandidateItem = CandidateBuilder.GetModel(rankingVote.PrimeCandidate),
                SubCandidateItem = CandidateBuilder.GetModel(rankingVote.SubCandidate),
                IsWriteIn = rankingVote.IsWriteIn
            };
        }

        public RankingVote GetEntity(RankingVoteItem rankingVote)
        {
            return new RankingVote()
            {
                RankingVoteId = rankingVote.RankingVoteItemId,
                PrimeCandidate = CandidateBuilder.GetEntity(rankingVote.PrimeCandidateItem),
                SubCandidate = CandidateBuilder.GetEntity(rankingVote.SubCandidateItem),
                IsWriteIn = rankingVote.IsWriteIn
            };
        }
    }
}
