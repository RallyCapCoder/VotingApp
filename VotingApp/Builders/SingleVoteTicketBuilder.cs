using VotingApp.Context;
using VotingApp.Models;

namespace VotingApp.Builders
{
    public class SingleVoteTicketBuilder
    {
        public CandidateBuilder CandidateBuilder { get; set; }
        public BallotIssueBuilder VoteIssueBuilder { get; set; }
        public SingleVoteTicketBuilder()
        {
            CandidateBuilder = new CandidateBuilder();
            VoteIssueBuilder = new BallotIssueBuilder();
        }

        public SingleVoteItem GetModel(SingleVote singleVote)
        {
            var item = new SingleVoteItem();
            item.SingleVoteTicketId = singleVote.SingleVoteId;
            if (singleVote.CandidateId != null)
            {
                item.CandidateItem = CandidateBuilder.GetModel(singleVote.Candidate);
            }
            if (singleVote.VoteIssueId != null)
            {
                item.Issue = VoteIssueBuilder.GetModel(singleVote.BallotIssue);
            }
            return item;
        }
    }
}
