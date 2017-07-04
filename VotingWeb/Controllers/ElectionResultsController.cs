using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using VotingApp.Managers;
using VotingWeb.Models;

namespace VotingWeb.Controllers
{
    public class ElectionResultsController : Controller
    {
        private static readonly Logger Log = LogManager.GetLogger("VoteLog");
        [Authorize(Roles = RoleName.CanSeeElectionResults)]
        public ActionResult Index()
        {

            if (!User.IsInRole(RoleName.CanSeeElectionResults)) return RedirectToAction("Index", "Home");
            Log.Info("Admin Viewing Results");
            var manager = new VotingManager();
            var viewModel = new ElectionResultsViewModel();

            var results = manager.GetVoteResults();

            viewModel.PresidentResults = manager.RankedVotingManager.GetRankingResults(results);
            viewModel.GetNumberOfRankings();
            var singleVoteItems = manager.SingleVoteManager.GetSingleVoteResults(results);
            viewModel.SupremeCourtResult = singleVoteItems.Where(x => x.Key.CandidateItem != null).ToDictionary(x => x.Key, x => x.Value);
            viewModel.BallotIssue = singleVoteItems.Where(x => x.Key.Issue != null).ToDictionary(x => x.Key, x => x.Value);
            viewModel.StateReps = manager.MultiVoteManager.GetMultiVoteResults(results);
            return View(viewModel);
        }

    }
}
