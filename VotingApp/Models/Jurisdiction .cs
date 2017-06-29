using System;

namespace VotingApp.Models
{
    public class Jurisdiction
    {
        public Guid JurisdictionId { get; set; }
        public string JurisdictionName { get; set; }
        public Guid BallotId { get; set; }
    }
}
