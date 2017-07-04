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
    public class SingleVoteManager
    {
        private VotingContext Context { get; set; }
        public SingleVoteManager(VotingContext context)
        {
            Context = context;
        }

        public List<SingleVoteItem> GetSingleVoteItems()
        {
            var builder = new SingleVoteTicketBuilder();
            var items = Context.SingleVotes
                .Include("BallotIssue")
                .Include("Candidate").Include("Candidate.Job")
                .Select(builder.GetModel).ToList();
            return items;
        }


        public List<SingleVoteItem> GetSingleVoteItemsByIds(List<Guid?> ids)
        {
            var builder = new SingleVoteTicketBuilder();
            var items = Context.SingleVotes
                .Include("BallotIssue")
                .Include("Candidate").Include("Candidate.Job")
                .Select(builder.GetModel).ToList();

            items = items.Where(x => ids.Contains(x.SingleVoteTicketId)).ToList();
            return items;
        }


        public Dictionary<SingleVoteItem, Dictionary<bool, int>> GetSingleVoteResults(List<VoteResults> voteResults)
        {

            var electionResultsForSingleVoteItems = new Dictionary<Guid, SingleVoteResults>();

            var singleVoteItems = voteResults.Where(x => x.SingleVoteId != null).ToList();

            foreach (var singleVoteItem in singleVoteItems)
            {

                if (singleVoteItem.SingleVoteId != null && (singleVoteItem.SingleVoteItem != null &&
                                                            electionResultsForSingleVoteItems.ContainsKey((Guid)singleVoteItem.SingleVoteId)))
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
                    var firstEntry = new Dictionary<bool, int> { { true, 0 }, { false, 0 } };
                    if (singleVoteItem.VotedYes != null && singleVoteItem.VotedYes.Value)
                    {
                        firstEntry[true]++;
                    }
                    if (singleVoteItem.VotedNo != null && singleVoteItem.VotedNo.Value)
                    {
                        firstEntry[false]++;
                    }
                    if (singleVoteItem.SingleVoteItem == null) continue;
                    if (singleVoteItem.SingleVoteId != null)
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

    }
}
