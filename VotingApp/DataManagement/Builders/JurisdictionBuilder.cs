using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VotingApp.DataManagement.Builders
{
    public class JurisdictionBuilder
    {
        public Models.Jurisdiction GetModel(Jurisdiction jurisdiction)
        {
            return new Models.Jurisdiction()
            {
                JurisdictionId = jurisdiction.JurisdictionId,
                JurisdictionName = jurisdiction.JurisdictionName,
            };
        }

        public Jurisdiction GetEntity(Models.Jurisdiction jurisdiction)
        {
            return new Jurisdiction()
            {
                JurisdictionId = Guid.NewGuid(),
                JurisdictionName = jurisdiction.JurisdictionName,
            };
        }
    }
}
