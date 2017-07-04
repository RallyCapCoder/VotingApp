using System;
using VotingApp.Context;

namespace VotingApp.Builders
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
