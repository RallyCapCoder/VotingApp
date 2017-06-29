using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.DataManagement.Builders
{
    public class BallotBuilder
    {
        public Models.Ballot GetModel(Ballot ballot)
        {
            return new Models.Ballot
            {
                BallotId = ballot.BallotId,
                BallotName = ballot.BallotName,
                State = ballot.State
            };
        }

        public Ballot GetEntity(Models.Ballot ballot)
        {
            return new Ballot
            {
                BallotId = Guid.NewGuid(),
                BallotName = ballot.BallotName,
                State = ballot.State
            };
        }
    }
}
