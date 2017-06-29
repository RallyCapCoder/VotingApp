using System;

namespace VotingApp.Models
{
    public class Ballot
    {
        public Guid BallotId { get; set; }
        public string BallotName { get; set; }
        public string State { get; set; }
        public DateTime DateModified { get; set; }
    }
}
