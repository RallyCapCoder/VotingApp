using System;
using System.Collections.Generic;
using System.Linq;
using VotingApp.Builders;
using VotingApp.Context;
using VotingApp.Models;

namespace VotingApp.Managers
{
    public class MultiVoteManager
    {
        private VotingContext Context { get; set; }

        public MultiVoteManager(VotingContext context)
        {
            Context = context;
        }

        public List<MultipleVoteItem> GetMultiVoteItems()
        {
            var builder = new MultiVoteTicketBuilder();
            var items = Context.MultipleVotes
                .Include("Candidate").Include("Candidate.Job")
                .Select(builder.GetModel).Where(x => !x.IsWriteIn).ToList();
            return items;
        }

        public List<MultipleVoteItem> GetMultiVoteItemsByIds(List<Guid?> ids)
        {
            var builder = new MultiVoteTicketBuilder();
            var items = Context.MultipleVotes
                .Include("Candidate").Include("Candidate.Job")
                .Select(builder.GetModel).ToList();
            items = items.Where(x => ids.Contains(x.MultipleVoteItemId)).ToList();
            return items;
        }

        public Dictionary<MultipleVoteItem, int> GetMultiVoteResults(List<VoteResults> voteResults)
        {

            var electionResultsForMultiVoteItems = new Dictionary<Guid, MultiVoteResults>();

            var multiVoteItems = voteResults.Where(x => x.MultipleVoteId != null).ToList();

            foreach (var multiVoteItem in multiVoteItems)
            {

                if (multiVoteItem.MultipleVoteId != null && (multiVoteItem.MultipleVoteItem != null &&
                                                             electionResultsForMultiVoteItems.ContainsKey((Guid)multiVoteItem.MultipleVoteId)))
                {
                    if (multiVoteItem.VotedFor != null && (bool)multiVoteItem.VotedFor)
                    {
                        electionResultsForMultiVoteItems[(Guid)multiVoteItem.MultipleVoteId].Votes = electionResultsForMultiVoteItems[(Guid)multiVoteItem.MultipleVoteId].Votes + 1;
                    }
                }
                else
                {
                    if (multiVoteItem.MultipleVoteItem == null) continue;
                    if (multiVoteItem.VotedFor != null && (bool)multiVoteItem.VotedFor)
                    {
                        if (multiVoteItem.MultipleVoteId != null)
                            electionResultsForMultiVoteItems.Add((Guid) multiVoteItem.MultipleVoteId,
                                new MultiVoteResults
                                {
                                    MultipleVoteItem = multiVoteItem.MultipleVoteItem,
                                    Votes = 1
                                });
                    }
                    else
                    {
                        if (multiVoteItem.MultipleVoteId != null)
                            electionResultsForMultiVoteItems.Add((Guid) multiVoteItem.MultipleVoteId,
                                new MultiVoteResults
                                {
                                    MultipleVoteItem = multiVoteItem.MultipleVoteItem,
                                    Votes = 0
                                });
                    }
                }
            }
            return electionResultsForMultiVoteItems.OrderBy(x => x.Value.Votes).ToDictionary(x => x.Value.MultipleVoteItem, x => x.Value.Votes);
        }

        public List<VoteResult> AddMultiVoteWriteInToElection(List<VoteResult> electionResults, MultipleVoteItem voteItem, Guid ballotId, MultipleVoteItem existingVoteItem)
        {
            var builder = new MultiVoteTicketBuilder();
            var multiVoteItem = new MultipleVoteItem()
            {
                MultipleVoteItemId = Guid.NewGuid(),
                CandidateItem = new CandidateItem()
                {
                    CandidateId = Guid.NewGuid(),
                    Name = voteItem.CandidateItem.Name,
                    JobId = existingVoteItem.CandidateItem.JobId
                },
                IsWriteIn = true
            };

            Context.MultipleVotes.Add(builder.GetEntity(multiVoteItem));
            Context.SaveChanges();

            electionResults.Add(new VoteResult
            {
                VoteResultsId = Guid.NewGuid(),
                BallotId = ballotId,
                MultipleVoteId = multiVoteItem.MultipleVoteItemId,
                VotedFor = voteItem.VotedFor,
            });


            return electionResults;
        }


    }
}
