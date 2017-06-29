using System;
using System.Collections.Generic;
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
        public void GetBallot()
        {
         
            var _context = new VotingBooth();
            var ballot = _context.Ballots.FirstOrDefault();
            Assert.IsNotNull(ballot);

        }

        [TestMethod]
        public void GetJurisdiction()
        {
            var _context = new VotingBooth();
            var jurisdiction = _context.Jurisdictions.FirstOrDefault();
            Assert.IsNotNull(jurisdiction);

        }

        [TestMethod]
        public void TESTING()
        {
            var _context = new VotingBooth();
            _context.VoteResults.Add(new VoteResult
            {
                CanindateId = Guid.Parse("6DD7FA77-3918-4F1F-A494-02540DD2D53A"),
                BallotId = Guid.Parse("8548CCAB-B89B-4DA4-9A2B-08029DFF1494"),
                VoteResultsId = Guid.NewGuid(),
                Ranking = 0,
                VotedFor = false
            });
            _context.SaveChanges();

        }

        [TestMethod]
        public void Vote_CIC_DemocratWin()
        {
            var _context = new VotingBooth();
            var _helper = new TestHelper();

            var ballotBuilder = new BallotBuilder();

            var ballot = _context.Ballots.First(x => x.BallotName == "Democrats Win the Ballot");
            var canindates = _helper.GetCanidates();

            var job = _helper.GetJob("Commander and Cream");

            var election = canindates.Where(x => x.JobId == job.JobId);

            var electionResults = new List<VoteResults>();

            foreach (var elCanidate in election)
            {
                elCanidate.Ranking = elCanidate.Party == "Democrat" ? 1 : 2;
                electionResults.Add(new VoteResults()
                {
                    BallotId = ballot.BallotId,
                    CanidateId = elCanidate.CanidateId,
                    Ranking = elCanidate.Ranking,
                    VotedFor = elCanidate.VotedFor
                });
            }


            var results = _helper.CreateElectionResult(electionResults);
            _context.VoteResults.AddRange(results.AsEnumerable());
            _context.SaveChanges();

            var finalTally = _context.VoteResults.FirstOrDefault(x => x.Ranking == 1);

            var electionWinner = _context.Canidates.First(x => x.CanidateId == finalTally.CanindateId);

            Assert.AreEqual(electionWinner.Party, "Democrat");

        }
    }
}
