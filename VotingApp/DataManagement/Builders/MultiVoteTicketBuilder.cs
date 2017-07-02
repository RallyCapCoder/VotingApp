using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Models;

namespace VotingApp.DataManagement.Builders
{
    public class MultiVoteTicketBuilder
    {
        public CanidateBuilder CanidateBuilder { get; set; }

        public MultiVoteTicketBuilder()
        {
            CanidateBuilder = new CanidateBuilder();
        }

        public MultipleVoteItem GetModel(MultipleVote multipleVote)
        {
            return new MultipleVoteItem
            {
                MultipleVoteItemId = multipleVote.MultipleVoteId,
                Canidate = CanidateBuilder.GetModel(multipleVote.Canidate)
            };
        }
    }
}
