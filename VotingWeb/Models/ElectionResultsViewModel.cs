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
            PreseidentResult = new List<RankedResults>();
            SupremeCourtResult = new SingleVoteResults();
            StateRepResult = new List<MultipleVotesResult>();
            BallotIssueResults = new SingleVoteResults();
        }

        public List<RankedResults> PreseidentResult { get; set; }
        public SingleVoteResults SupremeCourtResult { get; set; }
        public List<MultipleVotesResult> StateRepResult { get; set; }
        public SingleVoteResults BallotIssueResults { get; set; }

    }
}