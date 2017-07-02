using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotingApp.Models;

namespace VotingWeb.Models
{
    public class ElectionResultsViewModel
    {


        public List<VoteResults> VoteResults { get; set; }

        public Dictionary<RankingVoteItem, Dictionary<int, int>> PresidentResults { get; set; }

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