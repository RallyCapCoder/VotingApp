using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.DataManagement.Builders
{
    public class CanidateBuilder
    {
        public Models.Canidate GetModel(Canidate canidate)
        {
            return new Models.Canidate()
            {
                CanidateId = canidate.CanidateId,
                JobId = canidate.Job.JobId,
                JobName = canidate.Job.Name,
                Name = canidate.Name,
                Party = canidate.Party
            };
        }

        public Canidate GetEntity(Models.Canidate canidate)
        {
            return new Canidate()
            {
                CanidateId = canidate.CanidateId,
                JobId = canidate.JobId,
                Name = canidate.Name,
                Party = canidate.Party
            };
        }
    }
}
