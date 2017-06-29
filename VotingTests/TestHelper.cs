using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VotingApp.Models;

namespace VotingTests
{
    public class TestHelper
    {
        public Ballot CreateBallot()
        {
            var ballot = new VotingApp.Models.Ballot()
            {
                BallotId = Guid.NewGuid(),
                BallotName = "Testing Votem Ballot",
                DateModified = DateTime.Now,
                State = "OH"
            };

            return ballot;
        }

        public Jurisdiction CreateJurisdiction()
        { 
            var juris = new Jurisdiction
            {
                JurisdictionId = Guid.NewGuid(),
                JurisdictionName = "Federal And State"
            };
            return juris;
        }
    }
}
