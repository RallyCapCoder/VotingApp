using System;

namespace VotingApp.DataManagement.Builders
{
    public class VoteResultsBuilder
    {
        public Models.VoteResults GetModel(VoteResult voteResult)
        {
       
            return new Models.VoteResults()
            {
                VoteResultsId = voteResult.VoteResultsId,
                BallotId = voteResult.BallotId,
                CanidateId = voteResult.CanindateId,
                VoteIssueId = voteResult.VoteIssueId,
                Ranking = (int) voteResult.Ranking,
                VotedFor = voteResult.VotedFor
                
            };
        }

        public VoteResult GetEntity(Models.VoteResults voteResult)
        {
            return new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = voteResult.BallotId,
                CanindateId = voteResult.CanidateId,
                VoteIssueId = voteResult.VoteIssueId,
                Ranking = (int)voteResult.Ranking,
                VotedFor = voteResult.VotedFor
            };
        }
    }
}
