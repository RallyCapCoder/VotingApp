using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VotingApp.Builders;
using VotingApp.Context;
using VotingApp.Managers;
using VotingApp.Models;

namespace VotingTests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void GetBallot()
        {
         
            var _context = new VotingContext();
            var ballot = _context.Ballots.FirstOrDefault();
            Assert.IsNotNull(ballot);

        }

        [TestMethod]
        public void GetJurisdiction()
        {
            var _context = new VotingContext();
            var jurisdiction = _context.Jurisdictions.FirstOrDefault();
            Assert.IsNotNull(jurisdiction);

        }

        [TestMethod]
        public void GetSingleVoteItems()
        {
            var _context = new VotingContext();
            var _manager = new VotingManager();

            var canidates = _manager.SingleVoteManager.GetSingleVoteItems();
        }

        [TestMethod]
        public void GetElectionResults()
        {
            var _context = new VotingContext();
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

        [TestMethod]
        public void GetSingleVoteElectionResults()
        {
            var _context = new VotingContext();
            var _manager = new VotingManager();

            var canidates = _manager.GetVoteResults();



            var electionResultsForSingleVoteItems = new Dictionary<SingleVoteItem, Dictionary<bool, int>>();

            var singleVoteItems = canidates.Where(x => x.SingleVoteId != null).ToList();

            foreach (var singleVoteItem in singleVoteItems)
            {

                if (singleVoteItem.SingleVoteItem != null &&
                    electionResultsForSingleVoteItems.ContainsKey(singleVoteItem.SingleVoteItem))
                {
                    if (singleVoteItem.VotedYes != null && singleVoteItem.VotedYes.Value)
                    {
                        electionResultsForSingleVoteItems[singleVoteItem.SingleVoteItem][true]++;
                    }
                    if (singleVoteItem.VotedNo != null && singleVoteItem.VotedNo.Value)
                    {
                        electionResultsForSingleVoteItems[singleVoteItem.SingleVoteItem][false]++;
                    }
                }
                else
                {
                    var firstEntry = new Dictionary<bool, int>();
                    firstEntry.Add(true, 0);
                    firstEntry.Add(false, 0);
                    if (singleVoteItem.VotedYes != null && singleVoteItem.VotedYes.Value)
                    {
                        firstEntry[true]++;
                    }
                    if (singleVoteItem.VotedNo != null && singleVoteItem.VotedNo.Value)
                    {
                        firstEntry[false]++;
                    }
                    if (singleVoteItem.SingleVoteItem != null)
                        electionResultsForSingleVoteItems.Add(singleVoteItem.SingleVoteItem, firstEntry);
                }
            }


            var test = electionResultsForSingleVoteItems;


        }

        [TestMethod]
        public void AddAWriteInCanididate()
        {
            var _context = new VotingContext();
            var _manager = new VotingManager();

            var builder = new RankingVoteTicketBuilder();

            var RankingVoteItem = new RankingVoteItem
            {
                RankingVoteItemId = Guid.NewGuid(),
                PrimeCandidateItem = new VotingApp.Models.CandidateItem()
                {
                   CandidateId = Guid.NewGuid(),
                   Name = "Test",
                   JobId = Guid.Parse("521C573E-91E8-47CA-ACBC-BF3D63706F29")
                },
                SubCandidateItem = new VotingApp.Models.CandidateItem()
                {
                    CandidateId = Guid.NewGuid(),
                    Name = "Vice Test",
                    JobId = Guid.Parse("057A2ED5-CDE9-44DB-9697-041B0F09555F")
                },
                Ranking = 1
            };

            _manager.Context.RankingVotes.Add(builder.GetEntity(RankingVoteItem));
            _manager.Context.SaveChanges();
             



        }

    }
}
