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
                Ranking = (int)voteResult.Ranking,
                VotedFor = voteResult.VotedFor
            };
        }
    }
}
