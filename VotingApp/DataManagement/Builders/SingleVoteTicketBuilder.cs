using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Models;

namespace VotingApp.DataManagement.Builders
{
    public class SingleVoteTicketBuilder
    {
        public CanidateBuilder CanidateBuilder { get; set; }
        public VoteIssueBuilder VoteIssueBuilder { get; set; }
        public SingleVoteTicketBuilder()
        {
            CanidateBuilder = new CanidateBuilder();
            VoteIssueBuilder = new VoteIssueBuilder();
        }

        public SingleVoteItem GetModel(SingleVote singleVote)
        {
            var item = new SingleVoteItem();
            item.SingleVoteTicketId = singleVote.SingleVoteId;
            if (singleVote.CanindateId != null)
            {
                item.Canidate = CanidateBuilder.GetModel(singleVote.Canidate);
            }
            if (singleVote.VoteIssueId != null)
            {
                item.Issue = VoteIssueBuilder.GetModel(singleVote.VoteIssue);
            }
            return item;
        }
    }
}
