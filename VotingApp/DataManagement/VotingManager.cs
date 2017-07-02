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
            var canidates = _context.Canidates.Include("Job").Where(x => x.JobId == jobId).Select(_builder.GetModel).ToList();
            return canidates;
        }
        public List<VotingApp.Models.Canidate> GetCanidatesByParty(string partyName)
        {
            var _builder = new CanidateBuilder();
            var canidates = _context.Canidates.Include("Job").Where(x => x.Party == partyName).Select(_builder.GetModel).ToList();
            return canidates;
        }

        public List<VotingApp.Models.Canidate> GetAllCanidates()
        {
            var _builder = new CanidateBuilder();
            var canidates = _context.Canidates
                            .Include("Job").Select(_builder.GetModel).ToList();
            return canidates;
        }

        public List<SingleVoteItem> GetSingleVoteItems()
        {
            var _builder = new SingleVoteTicketBuilder();
            var items = _context.SingleVotes
                .Include("VoteIssue")
                .Include("Canidate").Include("Canidate.Job")
                .Select(_builder.GetModel).ToList();
            return items;
        }

        public List<MultipleVoteItem> GetMultiVoteItems()
        {
            var _builder = new MultiVoteTicketBuilder();
            var items = _context.MultipleVotes
                .Include("Canidate").Include("Canidate.Job")
                .Select(_builder.GetModel).ToList();
            return items;
        }


        public List<RankingVoteItem> GetRankedVoteItems()
        {
            var _builder = new RankingVoteTicketBuilder();
            var items = _context.RankingVotes.Include("PrimeCanindate").Include("SubCanindate")
                .Include("PrimeCanindate.Job").Include("SubCanindate.Job")
                .Select(_builder.GetModel).ToList();
            return items;
        }


        public List<SingleVoteItem> GetSingleVoteItemsByIds(List<Guid?> ids)
        {
            var _builder = new SingleVoteTicketBuilder();
            var items = _context.SingleVotes
                .Include("VoteIssue")
                .Include("Canidate").Include("Canidate.Job")
                .Select(_builder.GetModel).ToList();

            items = items.Where(x => ids.Contains(x.SingleVoteTicketId)).ToList();
            return items;
        }

        public List<MultipleVoteItem> GetMultiVoteItemsByIds(List<Guid?> ids)
        {
            var _builder = new MultiVoteTicketBuilder();
            var items = _context.MultipleVotes
                .Include("Canidate").Include("Canidate.Job")
                .Select(_builder.GetModel).ToList();
            items = items.Where(x => ids.Contains(x.MultipleVoteItemId)).ToList();
            return items;
        }


        public List<RankingVoteItem> GetRankedVoteItemsByIds(List<Guid?> ids)
        {
            var _builder = new RankingVoteTicketBuilder();
            var items = _context.RankingVotes.Include("PrimeCanindate").Include("SubCanindate")
                .Include("PrimeCanindate.Job").Include("SubCanindate.Job")
                .Select(_builder.GetModel).ToList();

            items = items.Where(x => ids.Contains(x.RankingVoteItemId)).ToList();
            return items;
        }


        public Guid GetJobId(string jobName)
        {
            var job = _context.Jobs.First(x => x.Name == jobName);
            return job.JobId;
        }

        public Models.Job GetJobByJobName(string jobName)
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

        public Models.VoteIssue GetIssueByOfficalName(string name)
        {
            var _builder = new VoteIssueBuilder();
            return _context.VoteIssues.Where(x => x.OfficalName == name).Select(_builder.GetModel).FirstOrDefault();
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

        public Models.Ballot CreateBallot(string BallotName)
        {
            var builder = new BallotBuilder();
            var ballot = _context.Ballots.FirstOrDefault(x => x.BallotName == BallotName);
            if (ballot != null)
            {
                return builder.GetModel(ballot);
            }
            ballot = new Ballot()
            {
                BallotId = Guid.NewGuid(),
                BallotName = BallotName
            };
            _context.Ballots.Add(ballot);
            _context.SaveChanges();

            return builder.GetModel(ballot);
        }

        public List<VoteResults> GetVoteResults()
        {
            var _builder = new VoteResultsBuilder();
            return _context.VoteResults
                .Include("RankingVote.PrimeCanindate.Job")
                .Include("RankingVote.SubCanindate.Job")
                .Include("SingleVote.Canidate.Job")
                .Include("SingleVote.VoteIssue")
                .Include("MultipleVote.Canidate.Job")
                .Select(_builder.GetModel).ToList();
        }

        public Dictionary<RankingVoteItem, Dictionary<int, int>> GetRankingResults(List<VoteResults> voteResults)
        {

            var electionResultsForPresidents = new Dictionary<RankingVoteItem, Dictionary<int, int>>();

            var presidentialCandidates = voteResults.Where(x => x.RankingVoteId != null).ToList();

            foreach (var presidentialCandidate in presidentialCandidates)
            {
                if (presidentialCandidate.RankingVoteId != null &&
                    electionResultsForPresidents.ContainsKey(presidentialCandidate.RankingVoteItem))
                {
                    if (electionResultsForPresidents[presidentialCandidate.RankingVoteItem]
                        .ContainsKey(presidentialCandidate.RankingVoteItem.Ranking))
                    {
                        var i = electionResultsForPresidents[presidentialCandidate.RankingVoteItem][
                            (int)presidentialCandidate.Ranking];
                        electionResultsForPresidents[presidentialCandidate.RankingVoteItem][
                            (int)presidentialCandidate.Ranking] = i++;
                        electionResultsForPresidents[presidentialCandidate.RankingVoteItem] = electionResultsForPresidents[presidentialCandidate.RankingVoteItem].OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else
                    {
                        electionResultsForPresidents[presidentialCandidate.RankingVoteItem].Add((int)presidentialCandidate.Ranking, 1);
                        electionResultsForPresidents[presidentialCandidate.RankingVoteItem] = electionResultsForPresidents[presidentialCandidate.RankingVoteItem].OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    }
                }
                else
                {
                    if (presidentialCandidate.RankingVoteId == null) continue;
                    var firstEntry = new Dictionary<int, int>();
                    firstEntry.Add((int)presidentialCandidate.Ranking, 1);
                    electionResultsForPresidents.Add(presidentialCandidate.RankingVoteItem, firstEntry);
                    electionResultsForPresidents[presidentialCandidate.RankingVoteItem] = electionResultsForPresidents[presidentialCandidate.RankingVoteItem].OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                }

            }
            return electionResultsForPresidents;
        }

        public void SaveElectionResults(List<VoteResult> results)
        {
            var _builder = new VoteResultsBuilder();

            _context.VoteResults.AddRange(results);
            _context.SaveChanges();
        }
    }
}
