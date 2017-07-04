using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingApp.Managers;
using VotingWeb.Models;

namespace VotingWeb.Controllers
{
    public class ElectionResultsController : Controller
    {
        [Authorize(Roles = RoleName.CanSeeElectionResults)]
        public ActionResult Index()
        {
            if (User.IsInRole(RoleName.CanSeeElectionResults))
            {
                var _manager = new VotingManager();
                var viewModel = new ElectionResultsViewModel();

                var results = _manager.GetVoteResults();

                viewModel.PresidentResults = _manager.RankedVotingManager.GetRankingResults(results);
                viewModel.GetNumberOfRankings();
                var singleVoteItems = _manager.SingleVoteManager.GetSingleVoteResults(results);
                viewModel.SupremeCourtResult = singleVoteItems.Where(x => x.Key.CandidateItem != null).ToDictionary(x => x.Key, x => x.Value);
                viewModel.BallotIssue = singleVoteItems.Where(x => x.Key.Issue != null).ToDictionary(x => x.Key, x => x.Value);
                viewModel.StateReps = _manager.MultiVoteManager.GetMultiVoteResults(results);
                return View(viewModel);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
