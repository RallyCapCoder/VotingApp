using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                .Select(_builder.GetModel).Where(x => !x.IsWriteIn).ToList();
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

        public Models.Ballot CreateBallot(string BallotName, string userId)
        {
            var builder = new BallotBuilder();
            var ballot = _context.Ballots.FirstOrDefault(x => x.AspNetUserId == userId);
            ballot = new Ballot()
            {
                BallotId = Guid.NewGuid(),
                BallotName = BallotName,
                AspNetUserId = userId
            };
            _context.Ballots.Add(ballot);
            _context.SaveChanges();

            return builder.GetModel(ballot);
        }

        public Models.Ballot FindExistingBallot(string userId)
        {
            var builder = new BallotBuilder();
            var ballot = _context.Ballots.FirstOrDefault(x => x.AspNetUserId == userId);
            return ballot == null ? null : builder.GetModel(ballot);
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

            var electionResultsForPresidents = new Dictionary<Guid, RankingVoteResults>();

            var presidentialCandidates = voteResults.Where(x => x.RankingVoteId != null).ToList();

            foreach (var presidentialCandidate in presidentialCandidates)
            {
                if (presidentialCandidate.RankingVoteId != null &&
                    electionResultsForPresidents.ContainsKey((Guid) presidentialCandidate.RankingVoteId))
                {
                    if (electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings.ContainsKey((int)presidentialCandidate.Ranking))
                    {
                        var i = electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings[
                            (int)presidentialCandidate.Ranking] + 1;
                        electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings[
                            (int)presidentialCandidate.Ranking] = i;
                        electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings = electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else
                    {
                        electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings.Add((int)presidentialCandidate.Ranking, 1);
                        electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings = electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    }
                }
                else
                {
                    if (presidentialCandidate.RankingVoteId == null) continue;
                    var firstEntry = new Dictionary<int, int>();
                    firstEntry.Add((int)presidentialCandidate.Ranking, 1);
                    electionResultsForPresidents.Add((Guid)presidentialCandidate.RankingVoteId, new RankingVoteResults{RankingVoteItem = presidentialCandidate.RankingVoteItem,Rankings = firstEntry});
                    electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings = electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                }

            }
            return electionResultsForPresidents.ToDictionary(x => x.Value.RankingVoteItem, x => x.Value.Rankings);
        }


        public Dictionary<SingleVoteItem, Dictionary<bool, int>> GetSingleVoteResults(List<VoteResults> voteResults)
        {
            
            var electionResultsForSingleVoteItems = new Dictionary<Guid, SingleVoteResults>();

            var singleVoteItems = voteResults.Where(x => x.SingleVoteId != null).ToList();

            foreach (var singleVoteItem in singleVoteItems)
            {

                if (singleVoteItem.SingleVoteItem != null &&
                    electionResultsForSingleVoteItems.ContainsKey((Guid)singleVoteItem.SingleVoteId))
                {
                    if (singleVoteItem.VotedYes != null && singleVoteItem.VotedYes.Value)
                    {
                        electionResultsForSingleVoteItems[(Guid)singleVoteItem.SingleVoteId].Votes[true]++;
                    }
                    if (singleVoteItem.VotedNo != null && singleVoteItem.VotedNo.Value)
                    {
                        electionResultsForSingleVoteItems[(Guid)singleVoteItem.SingleVoteId].Votes[false]++;
                    }
                }
                else
                {
                    var firstEntry = new Dictionary<bool, int> {{true, 0}, {false, 0}};
                    if (singleVoteItem.VotedYes != null && singleVoteItem.VotedYes.Value)
                    {
                        firstEntry[true]++;
                    }
                    if (singleVoteItem.VotedNo != null && singleVoteItem.VotedNo.Value)
                    {
                        firstEntry[false]++;
                    }
                    if (singleVoteItem.SingleVoteItem != null)
                        electionResultsForSingleVoteItems.Add((Guid) singleVoteItem.SingleVoteId,
                            new SingleVoteResults()
                            {
                                SingleVoteItem = singleVoteItem.SingleVoteItem,
                                Votes = firstEntry
                            });
                }
            }
            return electionResultsForSingleVoteItems.ToDictionary(x => x.Value.SingleVoteItem, x => x.Value.Votes);
        }


        public Dictionary<MultipleVoteItem,int> GetMultiVoteResults(List<VoteResults> voteResults)
        {

            var electionResultsForMultiVoteItems = new Dictionary<Guid, MultiVoteResults>();

            var multiVoteItems = voteResults.Where(x => x.MultipleVoteId != null).ToList();

            foreach (var multiVoteItem in multiVoteItems)
            {

                if (multiVoteItem.MultipleVoteItem != null &&
                    electionResultsForMultiVoteItems.ContainsKey((Guid)multiVoteItem.MultipleVoteId))
                {
                    if ((bool)multiVoteItem.VotedFor)
                    {
                        electionResultsForMultiVoteItems[(Guid)multiVoteItem.MultipleVoteId].Votes = electionResultsForMultiVoteItems[(Guid)multiVoteItem.MultipleVoteId].Votes + 1;
                    }
                }
                else
                {
                    if (multiVoteItem.MultipleVoteItem != null)
                        if ((bool) multiVoteItem.VotedFor)
                        {
                            electionResultsForMultiVoteItems.Add((Guid)multiVoteItem.MultipleVoteId, new MultiVoteResults{MultipleVoteItem = multiVoteItem.MultipleVoteItem , Votes = 1});
                        }
                        else
                        {
                            electionResultsForMultiVoteItems.Add((Guid)multiVoteItem.MultipleVoteId, new MultiVoteResults { MultipleVoteItem = multiVoteItem.MultipleVoteItem, Votes = 0 });
                        }
                       
                }
            }
            return electionResultsForMultiVoteItems.ToDictionary(x => x.Value.MultipleVoteItem, x => x.Value.Votes);
        }

        public List<VoteResult> AddRankingWriteInToElection(List<VoteResult> electionResults, RankingVoteItem voteItem, Guid ballotId, RankingVoteItem existingVoteItem)
        {
            var builder = new RankingVoteTicketBuilder();
            var RankingVoteItem = new RankingVoteItem
            {
                RankingVoteItemId = Guid.NewGuid(),
                PrimeCanidate = new VotingApp.Models.Canidate()
                {
                    CanidateId = Guid.NewGuid(),
                    Name = voteItem.PrimeCanidate.Name,
                    JobId = existingVoteItem.PrimeCanidate.JobId
                },
                SubCanidate = new VotingApp.Models.Canidate()
                {
                    CanidateId = Guid.NewGuid(),
                    Name = voteItem.SubCanidate.Name,
                    JobId = existingVoteItem.SubCanidate.JobId
                },
                IsWriteIn = true
            };

            _context.RankingVotes.Add(builder.GetEntity(RankingVoteItem));
            _context.SaveChanges();

            electionResults.Add(new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = ballotId,
                RankingVoteId = RankingVoteItem.RankingVoteItemId,
                Ranking = voteItem.Ranking,
            });


            return electionResults;
        }

        public void SaveElectionResults(List<VoteResult> results)
        {
            var _builder = new VoteResultsBuilder();

            _context.VoteResults.AddRange(results);
            _context.SaveChanges();
        }
    }
}
