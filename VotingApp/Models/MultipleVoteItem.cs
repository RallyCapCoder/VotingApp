using System;

namespace VotingApp.Models
{
    public class MultipleVoteItem
    {
        public Guid MultipleVoteItemId { get; set; }
        public Canidate Canidate { get; set; }
        public bool VotedFor { get; set; }
        public bool IsWriteIn { get; set; }
    }
}
