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
            var _builder = new RankingVoteTicketBuilder();
            var items = Context.RankingVotes.Include("PrimeCandidate").Include("SubCandidate")
                .Include("PrimeCandidate.Job").Include("SubCandidate.Job")
                .Select(_builder.GetModel).Where(x => !x.IsWriteIn).ToList();
            return items;
        }

        public List<RankingVoteItem> GetRankedVoteItemsByIds(List<Guid?> ids)
        {
            var _builder = new RankingVoteTicketBuilder();
            var items = Context.RankingVotes.Include("PrimeCandidate").Include("SubCandidate")
                .Include("PrimeCandidate.Job").Include("SubCandidate.Job")
                .Select(_builder.GetModel).ToList();

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
            var builder = new RankingVoteTicketBuilder();
            var RankingVoteItem = new RankingVoteItem
            {
                RankingVoteItemId = Guid.NewGuid(),
                PrimeCandidateItem = new VotingApp.Models.CandidateItem()
                {
                    CandidateId = Guid.NewGuid(),
                    Name = voteItem.PrimeCandidateItem.Name,
                    JobId = existingVoteItem.PrimeCandidateItem.JobId
                },
                SubCandidateItem = new VotingApp.Models.CandidateItem()
                {
                    CandidateId = Guid.NewGuid(),
                    Name = voteItem.SubCandidateItem.Name,
                    JobId = existingVoteItem.SubCandidateItem.JobId
                },
                IsWriteIn = true
            };

            Context.RankingVotes.Add(builder.GetEntity(RankingVoteItem));
            Context.SaveChanges();

            electionResults.Add(new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = ballotId,
                RankingVoteId = RankingVoteItem.RankingVoteItemId,
                Ranking = voteItem.Ranking,
            });


            return electionResults;
        }
    }
}
