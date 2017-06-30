using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.DataManagement.Builders;
using VotingApp.Models;

namespace VotingApp.DataManagement
{
    public class VotingManager
    {
        public VotingBooth _context { get; set; }

        public VotingManager()
        {
            _context = new VotingBooth();
        }

        public List<VotingApp.Models.Canidate> GetCanidatesByJobId(Guid jobId)
        {
            var _builder = new CanidateBuilder();
            var canidates = _context.Canidates.Where(x => x.JobId == jobId).Select(_builder.GetModel).ToList();
            return canidates;
        }
        public List<VotingApp.Models.Canidate> GetCanidatesByParty(string partyName)
        {
            var _builder = new CanidateBuilder();
            var canidates = _context.Canidates.Where(x => x.Party == partyName).Select(_builder.GetModel).ToList();
            return canidates;
        }

        public Guid GetJobId(string jobName)
        {
            var job = _context.Jobs.First(x => x.Name == jobName);
            return job.JobId;
        }

        public Models.Job GetJob(string jobName)
        {
            var _builder = new JobBuilder();
            var job = _builder.GetModel(_context.Jobs.First(x => x.Name == jobName));
            return job;
        }

        public List<Models.Job> GetAllJobs()
        {
            var _builder = new JobBuilder();
            var jobs = _context.Jobs.Select(_builder.GetModel).ToList();
            return jobs;
        }

        public Guid GetBallotByName(string ballotName)
        {
            return _context.Ballots.First(x => x.BallotName == ballotName).BallotId;
        }

        public CanindateParty FillCanindateParty(string partyToFill)
        {

            CanindateParty electionGroup = new CanindateParty();

            var jobs = GetAllJobs();

            var canindates = GetCanidatesByParty(partyToFill);

            foreach (var job in jobs)
            {
                var toBeElected = canindates.FirstOrDefault(x => x.JobId == job.JobId);
                if (toBeElected == null) continue;
                switch (job.Name)
                {
                    case "Commander and Cream":
                        electionGroup.President = toBeElected;
                        break;
                    case "Vice Ice":
                        electionGroup.VicePresident = toBeElected;
                        break;
                    case "Chief Dairy Queen":
                        electionGroup.SupremeCourtJustice = toBeElected;
                        break;
                    case "State Rep District M&M":
                        electionGroup.StateRep = toBeElected;
                        break;
                }
            }
            return electionGroup;

        }

        public void CreateBallot(string BallotName)
        {
            var _builder = new BallotBuilder();
            _context.Ballots.Add(new Ballot
            {
                BallotId = Guid.NewGuid(),
                BallotName = BallotName
            });
            _context.SaveChanges();
        }

        public void SaveElectionResults(List<VoteResult> results)
        {
            var _builder = new VoteResultsBuilder();

            _context.VoteResults.AddRange(results);
            _context.SaveChanges();
        }
    }
}
