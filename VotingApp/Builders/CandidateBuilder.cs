using VotingApp.Context;

namespace VotingApp.Builders
{
    public class CandidateBuilder
    {
        public Models.CandidateItem GetModel(Candidate canidate)
        {
            return new Models.CandidateItem()
            {
                CandidateId = canidate.CandidateId,
                JobId = canidate.Job.JobId,
                JobName = canidate.Job.Name,
                Name = canidate.Name,
                Party = canidate.Party
            };
        }

        public Candidate GetEntity(Models.CandidateItem candidateItem)
        {
            return new Candidate()
            {
                CandidateId = candidateItem.CandidateId,
                JobId = candidateItem.JobId,
                Name = candidateItem.Name,
                Party = candidateItem.Party
            };
        }
    }
}
