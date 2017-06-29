using System;

namespace VotingApp.Models
{
    public class Job
    {
        public Guid JobId { get; set; }
        public Guid JurisdictionId { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public int TypeOfVoting { get; set; }

    }
}
