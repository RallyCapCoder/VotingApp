using System;
using System.Collections.Generic;
using System.Linq;
using VotingApp.Builders;
using VotingApp.Context;
using VotingApp.Models;
using Ballot = VotingApp.Context.Ballot;

namespace VotingApp.Managers
{
    public class VotingManager
    {
        public VotingContext Context { get; set; }
        public RankedVotingManager RankedVotingManager { get; set; }
        public SingleVoteManager SingleVoteManager { get; set; }
        public MultiVoteManager MultiVoteManager { get; set; }

        public VotingManager()
        {
            Context = new VotingContext();
            RankedVotingManager = new RankedVotingManager(Context);
            SingleVoteManager = new SingleVoteManager(Context);
            MultiVoteManager = new MultiVoteManager(Context);
        }

        public Models.Ballot CreateBallot(string ballotName, string userId)
        {
            var builder = new BallotBuilder();
            var ballot = new Ballot()
            {
                BallotId = Guid.NewGuid(),
                BallotName = ballotName,
                AspNetUserId = userId
            };
            Context.Ballots.Add(ballot);
            Context.SaveChanges();

            return builder.GetModel(ballot);
        }

        public Models.Ballot FindExistingBallot(string userId)
        {
            var builder = new BallotBuilder();
            var ballot = Context.Ballots.FirstOrDefault(x => x.AspNetUserId == userId);
            return ballot == null ? null : builder.GetModel(ballot);
        }

        public List<VoteResults> GetVoteResults()
        {
            var builder = new VoteResultsBuilder();
            return Context.VoteResults
                .Include("RankingVote.PrimeCandidate.Job")
                .Include("RankingVote.SubCandidate.Job")
                .Include("SingleVote.Candidate.Job")
                .Include("SingleVote.BallotIssue")
                .Include("MultipleVote.Candidate.Job")
                .Select(builder.GetModel).ToList();
        }

        public void SaveElectionResults(List<VoteResult> results)
        {
            Context.VoteResults.AddRange(results);
            Context.SaveChanges();
        }
    }
}
