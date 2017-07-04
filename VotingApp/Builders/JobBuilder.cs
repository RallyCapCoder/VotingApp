using System;
using VotingApp.Context;

namespace VotingApp.Builders
{
    public class JobBuilder
    {

        public Models.Job GetModel(Job job)
        {
            return new Models.Job()
            {
                JobId = job.JobId,
                JurisdictionId = job.Jurisdiction,
                Name = job.Name
            };
        }

        public Job GetEntity(Models.Job job)
        {
            return new Job()
            {
                JobId = Guid.NewGuid(),
                Jurisdiction = job.JurisdictionId,
                Name = job.Name
            };
        }
    }
}
