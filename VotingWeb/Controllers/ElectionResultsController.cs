using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingApp.DataManagement;
using VotingWeb.Models;

namespace VotingWeb.Controllers
{
    public class ElectionResultsController : Controller
    {
        public ActionResult Index()
        {
            var _manager = new VotingManager();
            var viewModel = new ElectionResultsViewModel();

            var results = _manager.GetVoteResults();

            viewModel.PresidentResults = _manager.GetRankingResults(results);
            viewModel.GetNumberOfRankings();
            var singleVoteItems = _manager.GetSingleVoteResults(results);
            viewModel.SupremeCourtResult = singleVoteItems.Where(x => x.Key.Canidate != null).ToDictionary(x => x.Key, x => x.Value);
            viewModel.BallotIssue = singleVoteItems.Where(x => x.Key.Issue != null).ToDictionary(x => x.Key, x => x.Value);
            viewModel.StateReps = _manager.GetMultiVoteResults(results);
            return View(viewModel);
        }

    }
}
