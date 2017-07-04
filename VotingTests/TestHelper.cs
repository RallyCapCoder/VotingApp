using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Builders;
using VotingApp.Context;
using Ballot = VotingApp.Models.Ballot;
using Jurisdiction = VotingApp.Models.Jurisdiction;

namespace VotingTests
{
    public class TestHelper
    {

        public Ballot CreateBallot(string ballotName )
        {
            var ballot = new VotingApp.Models.Ballot()
            {
                BallotId = Guid.NewGuid(),
                BallotName = ballotName
            };

            return ballot;
        }

        public Jurisdiction CreateJurisdiction()
        { 
            var juris = new Jurisdiction
            {
                JurisdictionId = Guid.NewGuid(),
                JurisdictionName = "Federal And State"
            };
            return juris;
        }

        public List<VotingApp.Models.CandidateItem> GetCanidates()
        {
            var _context = new VotingContext();
            var _builder = new CandidateBuilder();

            var canindates = _context.Candidates.AsNoTracking().Include("Job");

            return canindates.Select(_builder.GetModel).ToList();
        }

        public VotingApp.Models.Job GetJob(string jobName)
        {
            var _context = new VotingContext();
            var _builder = new JobBuilder();
            var job = _builder.GetModel(_context.Jobs.First(x => x.Name == jobName));

            return job;
        }

        public List<VoteResult> CreateElectionResult(List<VotingApp.Models.VoteResults> voteResults)
        {
            var _context = new VotingContext();
            var _builder = new VoteResultsBuilder();

            return voteResults.Select(_builder.GetEntity).ToList();

        }
    }
}
