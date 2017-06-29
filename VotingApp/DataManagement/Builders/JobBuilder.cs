using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.DataManagement.Builders
{
    public class JobBuilder
    {
        public Models.Job GetModel(Job job)
        {
            return new Models.Job()
            {
                JobId = job.JobId,
                JobTitle = job.JobTitle,
                JurisdictionId = job.JurisdictionId,
                Description = job.Description
            };
        }

        public Job GetEntity(Models.Job job)
        {
            return new Job()
            {
                JobId = Guid.NewGuid(),
                JobTitle = job.JobTitle,
                JurisdictionId = job.JurisdictionId,
                Description = job.Description
            };
        }
    }
}
