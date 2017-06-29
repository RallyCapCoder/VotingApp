using System;

namespace VotingApp.Models
{
    public class Canidate
    {
        public Guid CanidateId { get; set; }
        public string Name { get; set; }
        public Guid JobId { get; set; }
        public string JobName { get; set; }
        public string Party { get; set; }
        public bool VotedFor { get; set; }
        public int Ranking { get; set; }
    }
}
