using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingApp.DataManagement;
using VotingApp.DataManagement.Builders;
using VotingApp.Models;

namespace VotingTests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void CreateBallot()
        {
            var _context = new VotingBooth();
            var _builder = new BallotBuilder();

            var _helper = new TestHelper();
            var ballot = _helper.CreateBallot();

            _context.Ballots.Add(_builder.GetEntity(ballot));

            _context.Ballots.Add(_builder.GetEntity(ballot));
            _context.SaveChanges();
            var BallotName = "Testing Votem Ballot";
            var dbballot = _context.Ballots.FirstOrDefault(x => x.BallotName.Equals(BallotName));

            Assert.IsNotNull(dbballot);

            if (dbballot != null)
            {
                _context.Ballots.Remove(dbballot);
            }
            
        }

        [TestMethod]
        public void CreateJurisdiction()
        {
            var _context = new VotingBooth();
            var _builder = new BallotBuilder();

            var _helper = new TestHelper();
            var ballot = _helper.CreateBallot();

            _context.Ballots.Add(_builder.GetEntity(ballot));
            _context.SaveChanges();
            var BallotName = "Testing Votem Ballot";
            var dbballot = _context.Ballots.FirstOrDefault(x => x.BallotName.Equals(BallotName));

            var _jurisBuilder = new JurisdictionBuilder();

            var jurisdiction = _helper.CreateJurisdiction();
            jurisdiction.BallotId = dbballot.BallotId;

            _context.Jurisdictions.Add(_jurisBuilder.GetEntity(jurisdiction));
            _context.SaveChanges();

            var validJurisdiction = _context.Jurisdictions.Where(x => x.BallotId == dbballot.BallotId);

            Assert.IsNotNull(validJurisdiction);

            if (dbballot != null)
            {
                _context.Ballots.Remove(dbballot);
            }

        }
    }
}
