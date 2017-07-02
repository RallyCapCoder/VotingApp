using System;
using VotingApp.Models;

namespace VotingApp.DataManagement.Builders
{
    public class VoteResultsBuilder
    {
        private RankingVoteTicketBuilder RankingVoteTicketBuilder { get; set; }
        private SingleVoteTicketBuilder SingleVoteTicketBuilder { get; set; }
        private MultiVoteTicketBuilder MultiVoteTicketBuilder { get; set; }

        public VoteResultsBuilder()
        {
            RankingVoteTicketBuilder = new RankingVoteTicketBuilder();
            SingleVoteTicketBuilder = new SingleVoteTicketBuilder();
            MultiVoteTicketBuilder = new MultiVoteTicketBuilder();
        }
        public Models.VoteResults GetModel(VoteResult voteResult)
        {
            
            var electionResult = new VoteResults();
            if (voteResult.RankingVoteId != null)
            {
                electionResult.RankingVoteId = voteResult.RankingVoteId;
                electionResult.RankingVoteItem = RankingVoteTicketBuilder.GetModel(voteResult.RankingVote);
                electionResult.Ranking = voteResult.Ranking;
            }
            if (voteResult.SingleVote != null)
            {
                electionResult.SingleVoteId = voteResult.SingleVoteId;
                electionResult.SingleVoteItem = SingleVoteTicketBuilder.GetModel(voteResult.SingleVote);
                electionResult.VotedYes = voteResult.VoteYes;
                electionResult.VotedNo = voteResult.VoteNo;
            }
            if (voteResult.MultipleVote != null)
            {
                electionResult.MultipleVoteId = voteResult.MultipleVoteId;
                electionResult.MultipleVoteItem = MultiVoteTicketBuilder.GetModel(voteResult.MultipleVote);
                electionResult.VotedFor = voteResult.VotedFor;
            }
            return electionResult;

        }

        public VoteResult GetEntity(Models.VoteResults voteResult)
        {
            return new VoteResult
            {
                VoteResultsId = voteResult.VoteResultsId,
                BallotId = voteResult.BallotId,
                RankingVoteId = voteResult.RankingVoteId,
                MultipleVoteId = voteResult.MultipleVoteId,
                SingleVoteId = voteResult.SingleVoteId,
                Ranking = voteResult.Ranking ?? 0,
                VotedFor = voteResult.VotedFor,
                VoteYes = voteResult.VotedYes,
                VoteNo = voteResult.VotedNo
            };
        }
    }
}
