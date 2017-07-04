using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingApp.Models;

namespace VotingWeb.Models
{
    public class ElectionResultsViewModel
    {
        public ElectionResultsViewModel()
        {
            VoteResults = new List<VoteResults>();
            PresidentResults = new Dictionary<RankingVoteItem, Dictionary<int, int>>();
            SupremeCourtResult = new Dictionary<SingleVoteItem, Dictionary<bool, int>>();
            StateReps = new Dictionary<MultipleVoteItem, int>();
            BallotIssue = new Dictionary<SingleVoteItem, Dictionary<bool, int>>();
        }

        public List<VoteResults> VoteResults { get; set; }

        public Dictionary<RankingVoteItem, Dictionary<int, int>> PresidentResults { get; set; }

        public Dictionary<SingleVoteItem,Dictionary<bool,int>> SupremeCourtResult { get; set; }

        public Dictionary<MultipleVoteItem,int> StateReps { get; set; }

        public Dictionary<SingleVoteItem, Dictionary<bool, int>> BallotIssue { get; set; }

        public int NumberOfRankings { get; set; }


        public void GetNumberOfRankings()
        {
            NumberOfRankings = 0;
            foreach (var presidentResult in PresidentResults)
            {
                foreach (var rankCount in presidentResult.Value)
                {
                    if (rankCount.Key > NumberOfRankings)
                    {
                        NumberOfRankings = rankCount.Key;
                    }
                }
            }
        }

    }
}