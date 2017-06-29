using System;

namespace VotingApp.Models
{
    public class Job
    {
        public Guid JobId { get; set; }
        public Guid JurisdictionId { get; set; }
        public string Name { get; set; }
    }
}
