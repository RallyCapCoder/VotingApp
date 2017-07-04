using VotingApp.Context;
using VotingApp.Models;

namespace VotingApp.Builders
{
    public class MultiVoteTicketBuilder
    {
        public CandidateBuilder CandidateBuilder { get; set; }

        public MultiVoteTicketBuilder()
        {
            CandidateBuilder = new CandidateBuilder();
        }

        public MultipleVoteItem GetModel(MultipleVote multipleVote)
        {
            return new MultipleVoteItem
            {
                MultipleVoteItemId = multipleVote.MultipleVoteId,
                CandidateItem = CandidateBuilder.GetModel(multipleVote.Candidate),
                IsWriteIn = multipleVote.IsWriteIn
            };
        }

        public MultipleVote GetEntity(MultipleVoteItem multipleVote)
        {
            return new MultipleVote
            {
                MultipleVoteId = multipleVote.MultipleVoteItemId,
                Candidate = CandidateBuilder.GetEntity(multipleVote.CandidateItem),
                IsWriteIn = multipleVote.IsWriteIn
            };
        }
    }
}
