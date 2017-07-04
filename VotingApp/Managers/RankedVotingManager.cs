using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Builders;
using VotingApp.Context;
using VotingApp.Models;

namespace VotingApp.Managers
{
    public class RankedVotingManager
    {
        private VotingContext Context { get; set; }

        public RankedVotingManager(VotingContext context)
        {
            Context = context;
        }

        public List<RankingVoteItem> GetRankedVoteItems()
        {
            var builder = new RankingVoteTicketBuilder();
            var items = Context.RankingVotes.Include("PrimeCandidate").Include("SubCandidate")
                .Include("PrimeCandidate.Job").Include("SubCandidate.Job")
                .Select(builder.GetModel).Where(x => !x.IsWriteIn).ToList();
            return items;
        }

        public List<RankingVoteItem> GetRankedVoteItemsByIds(List<Guid?> ids)
        {
            var builder = new RankingVoteTicketBuilder();
            var items = Context.RankingVotes.Include("PrimeCandidate").Include("SubCandidate")
                .Include("PrimeCandidate.Job").Include("SubCandidate.Job")
                .Select(builder.GetModel).ToList();

            items = items.Where(x => ids.Contains(x.RankingVoteItemId)).ToList();
            return items;
        }

        public Dictionary<RankingVoteItem, Dictionary<int, int>> GetRankingResults(List<VoteResults> voteResults)
        {

            var electionResultsForPresidents = new Dictionary<Guid, RankingVoteResults>();

            var presidentialCandidates = voteResults.Where(x => x.RankingVoteId != null).ToList();

            foreach (var presidentialCandidate in presidentialCandidates)
            {
                if (presidentialCandidate.RankingVoteId != null &&
                    electionResultsForPresidents.ContainsKey((Guid)presidentialCandidate.RankingVoteId))
                {
                    if (presidentialCandidate.Ranking != null && electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings.ContainsKey((int)presidentialCandidate.Ranking))
                    {
                        var i = electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings[
                                    (int)presidentialCandidate.Ranking] + 1;
                        electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings[
                            (int)presidentialCandidate.Ranking] = i;
                        electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings = electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else
                    {
                        if (presidentialCandidate.Ranking != null)
                            electionResultsForPresidents[(Guid) presidentialCandidate.RankingVoteId].Rankings
                                .Add((int) presidentialCandidate.Ranking, 1);
                        electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings = electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    }
                }
                else
                {
                    if (presidentialCandidate.RankingVoteId == null) continue;
                    if (presidentialCandidate.Ranking != null)
                    {
                        var firstEntry = new Dictionary<int, int> {{(int) presidentialCandidate.Ranking, 1}};
                        electionResultsForPresidents.Add((Guid)presidentialCandidate.RankingVoteId, new RankingVoteResults { RankingVoteItem = presidentialCandidate.RankingVoteItem, Rankings = firstEntry });
                    }
                    electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings = electionResultsForPresidents[(Guid)presidentialCandidate.RankingVoteId].Rankings.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                }

            }
            return electionResultsForPresidents.ToDictionary(x => x.Value.RankingVoteItem, x => x.Value.Rankings);
        }

        public List<VoteResult> AddRankingWriteInToElection(List<VoteResult> electionResults, RankingVoteItem voteItem, Guid ballotId, RankingVoteItem existingVoteItem)
        {

          
            var primeCandidateItem = CheckForExistingPrimeCandidate(voteItem, existingVoteItem);
            var subCandidateItem = CheckForExistingSubCandidate(voteItem, existingVoteItem);

            var rankingVoteId =
                Context.RankingVotes.FirstOrDefault(x => x.PrimeCandidateId == primeCandidateItem.CandidateId ||
                                                x.SubCandidateId == subCandidateItem.CandidateId);
            if (rankingVoteId == null)
            {
                var rankingVoteItem = new RankingVoteItem
                {
                    RankingVoteItemId = Guid.NewGuid(),
                    PrimeCandidateItem = primeCandidateItem,
                    SubCandidateItem = subCandidateItem,
                    IsWriteIn = true
                };
                var builder = new RankingVoteTicketBuilder();
                Context.RankingVotes.Add(builder.GetEntity(rankingVoteItem));
                Context.SaveChanges();
                electionResults.Add(new VoteResult
                {
                    VoteResultsId = Guid.NewGuid(),
                    BallotId = ballotId,
                    RankingVoteId = rankingVoteItem.RankingVoteItemId,
                    Ranking = voteItem.Ranking,
                });
            }
            else
            {
                electionResults.Add(new VoteResult
                {
                    VoteResultsId = Guid.NewGuid(),
                    BallotId = ballotId,
                    RankingVoteId = rankingVoteId.RankingVoteId,
                    Ranking = voteItem.Ranking,
                });
            }

        


            return electionResults;
        }

        private CandidateItem CheckForExistingSubCandidate(RankingVoteItem voteItem, RankingVoteItem existingVoteItem)
        {
            var builder = new CandidateBuilder();
            var existingSubWriteIn =
                Context.Candidates.FirstOrDefault(x => x.Name == voteItem.SubCandidateItem.Name &&
                                                       x.JobId == existingVoteItem.SubCandidateItem.JobId);
            CandidateItem subCandidateItem;
            if (existingSubWriteIn != null)
            {
                subCandidateItem = builder.GetModel(existingSubWriteIn);
            }
            else
            {
                subCandidateItem = new VotingApp.Models.CandidateItem()
                {
                    CandidateId = Guid.NewGuid(),
                    Name = voteItem.SubCandidateItem.Name,
                    JobId = existingVoteItem.SubCandidateItem.JobId
                };
            }
            return subCandidateItem;
        }

        private CandidateItem CheckForExistingPrimeCandidate(RankingVoteItem voteItem, RankingVoteItem existingVoteItem)
        {
            var existingPrimeWriteIn =
                Context.Candidates.FirstOrDefault(x => x.Name == voteItem.PrimeCandidateItem.Name &&
                                                       x.JobId == existingVoteItem.PrimeCandidateItem.JobId);

            var builder = new CandidateBuilder();

            CandidateItem primeCandidateItem;


            if (existingPrimeWriteIn != null)
            {
                primeCandidateItem = builder.GetModel(existingPrimeWriteIn);
            }
            else
            {
                primeCandidateItem = new CandidateItem()
                {
                    CandidateId = Guid.NewGuid(),
                    Name = voteItem.PrimeCandidateItem.Name,
                    JobId = existingVoteItem.PrimeCandidateItem.JobId
                };
            }
            return primeCandidateItem;
        }
    }
}
