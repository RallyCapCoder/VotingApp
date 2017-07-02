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
        // GET: ElectionResults
        public ActionResult Index()
        {
            var _manager = new VotingManager();
            var viewModel = new ElectionResultsViewModel();

            var results = _manager.GetVoteResults();

            viewModel.PresidentResults = _manager.GetRankingResults(results);
            viewModel.GetNumberOfRankings();

            return View(viewModel);
        }

    }
}
