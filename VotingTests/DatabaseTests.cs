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
        public void GetCanindates()
        {
            var _context = new VotingBooth();
            var _manager = new VotingManager();

            var canidates = _manager.GetAllCanidates();
        }

        [TestMethod]
        public void GetSingleVoteItems()
        {
            var _context = new VotingBooth();
            var _manager = new VotingManager();

            var canidates = _manager.GetSingleVoteItems();
        }

        [TestMethod]
        public void GetElectionResults()
        {
            var _context = new VotingBooth();
            var _manager = new VotingManager();

            var canidates = _manager.GetVoteResults();



            var electionResultsForPresidents = new Dictionary<RankingVoteItem,Dictionary<int,int>>();

            var presidentialCandidates = canidates.Where(x => x.RankingVoteId != null).ToList();

            foreach (var presidentialCandidate in presidentialCandidates)
            {
                if (presidentialCandidate.RankingVoteId != null &&
                    electionResultsForPresidents.ContainsKey(presidentialCandidate.RankingVoteItem))
                {
                    if (electionResultsForPresidents[presidentialCandidate.RankingVoteItem]
                        .ContainsKey(presidentialCandidate.RankingVoteItem.Ranking))
                    {
                        var i = electionResultsForPresidents[presidentialCandidate.RankingVoteItem][
                            (int)presidentialCandidate.Ranking];
                        electionResultsForPresidents[presidentialCandidate.RankingVoteItem][
                            (int)presidentialCandidate.Ranking] = i++;
                        electionResultsForPresidents[presidentialCandidate.RankingVoteItem] = electionResultsForPresidents[presidentialCandidate.RankingVoteItem].OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    }
                    else
                    {
                        electionResultsForPresidents[presidentialCandidate.RankingVoteItem].Add((int)presidentialCandidate.Ranking,1);
                        electionResultsForPresidents[presidentialCandidate.RankingVoteItem] = electionResultsForPresidents[presidentialCandidate.RankingVoteItem].OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    }
                }
                else
                {
                    if (presidentialCandidate.RankingVoteId != null)
                    {
                        var firstEntry = new Dictionary<int, int>();
                        firstEntry.Add((int)presidentialCandidate.Ranking,1);
                        electionResultsForPresidents.Add(presidentialCandidate.RankingVoteItem,firstEntry);
                        electionResultsForPresidents[presidentialCandidate.RankingVoteItem] = electionResultsForPresidents[presidentialCandidate.RankingVoteItem].OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

                    }
                }

            }

          
            var test = electionResultsForPresidents;


        }

    }
}
